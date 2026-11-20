using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using PremiumLift.DataAccess;
using PremiumLift.Models;
using PremiumLift.UI;

namespace PremiumLift
{
    /// <summary>
    /// Main window of the Aurum Lift dual-elevator controller.
    ///
    ///   Task 1  GUI (MainForm.Designer.cs + UI controls) and event handlers
    ///   Task 2  Control logic: requests move the lifts, every display updates
    ///   Task 3  SQLite log via SQLiteDataAdapter.Update() (DataAccess folder)
    ///   Task 4  Animation: Timer + delegate inside Elevator, drawn here
    ///   Task 5  BackgroundWorker for the database (DatabaseWorker),
    ///           two lifts with State + Observer patterns (Models folder)
    ///
    /// The form is an OBSERVER of both lifts: whenever a lift changes,
    /// OnElevatorEvent() redraws that lift's car, displays and buttons.
    /// </summary>
    public partial class MainForm : Form, IElevatorObserver
    {
        /// <summary>All controls that show one lift, so A and B share the same code.</summary>
        private sealed class LiftView
        {
            public ElevatorCar Car;
            public ShaftPanel Shaft;
            public DigitalDisplay CarDisplay;
            public DigitalDisplay[] HallDisplays;   // index = floor
            public LuxButton[] FloorButtons;        // index = floor
            public Label Status;
        }

        private static readonly Color StatusOk = Color.FromArgb(123, 227, 163);
        private static readonly Color StatusBusy = Color.FromArgb(240, 214, 150);
        private static readonly Color StatusError = Color.FromArgb(255, 130, 120);

        private readonly Elevator _liftA;
        private readonly Elevator _liftB;
        private readonly ElevatorDispatcher _dispatcher;
        private readonly Dictionary<Elevator, LiftView> _views;
        private readonly LuxButton[] _hallButtons;          // index = floor

        private readonly ElevatorLogRepository _repository;
        private readonly DatabaseWorker _dbWorker;
        private readonly ElevatorLogger _logger;
        private readonly ToolTip _toolTip = new ToolTip();

        private bool _logVisible;
        private bool _logRefreshQueued;
        private bool _closeRequested;
        private bool _readyToClose;
        private DataTable _shownLog;

        public MainForm()
        {
            InitializeComponent();

            // ---- Lifts (Lift A parked at Ground floor G, Lift B at Floor 1) ----
            _liftA = new Elevator("A", 0);
            _liftB = new Elevator("B", 1);
            _dispatcher = new ElevatorDispatcher(_liftA, _liftB);

            _views = new Dictionary<Elevator, LiftView>
            {
                [_liftA] = new LiftView
                {
                    Car = carA, Shaft = pnlShaftA, CarDisplay = dispCarA,
                    HallDisplays = new[] { dispHall0A, dispHall1A },
                    FloorButtons = new[] { btnAFloor0, btnAFloor1 },
                    Status = lblStatusA
                },
                [_liftB] = new LiftView
                {
                    Car = carB, Shaft = pnlShaftB, CarDisplay = dispCarB,
                    HallDisplays = new[] { dispHall0B, dispHall1B },
                    FloorButtons = new[] { btnBFloor0, btnBFloor1 },
                    Status = lblStatusB
                }
            };
            _hallButtons = new[] { btnCall0, btnCall1 };

            // ---- Database (background) ----
            _repository = new ElevatorLogRepository();
            _dbWorker = new DatabaseWorker();
            _dbWorker.JobFailed += DbWorker_JobFailed;
            _logger = new ElevatorLogger(_repository, _dbWorker);
            _logger.EntrySaved += Logger_EntrySaved;

            // ---- Observer pattern: GUI and logger watch both lifts ----
            foreach (var lift in _dispatcher.Elevators)
            {
                lift.Attach(this);
                lift.Attach(_logger);
            }

            dgvLog.CellFormatting += dgvLog_CellFormatting;
            SetupToolTips();
        }

        // ==================================================================
        //  Form lifecycle
        // ==================================================================

        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (var lift in _dispatcher.Elevators)
                Render(lift);

            SetSystemStatus("●  CONNECTING TO LOG DATABASE…", StatusBusy);

            // First job in the queue, so every later save finds the table ready.
            _dbWorker.Enqueue("Open database", () => { _repository.EnsureDatabase(); return true; },
                              ok => SetSystemStatus("●  SYSTEM READY  ·  2 LIFTS ONLINE", StatusOk));
            _logger.Log("System", "Controller started", null);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_readyToClose) return;

            // Stop the lifts so no new log entries are created.
            foreach (var lift in _dispatcher.Elevators)
                lift.Stop();

            if (_dbWorker.IsBusy)
            {
                // Let the BackgroundWorker finish saving, then close.
                e.Cancel = true;
                if (!_closeRequested)
                {
                    _closeRequested = true;
                    SetSystemStatus("●  SAVING LOG…", StatusBusy);
                    _dbWorker.Drained += (s, args) => BeginInvoke(new Action(FinishClose));
                }
                return;
            }

            ReleaseResources();
        }

        private void FinishClose()
        {
            _readyToClose = true;
            ReleaseResources();
            Close();
        }

        private void ReleaseResources()
        {
            foreach (var lift in _dispatcher.Elevators)
            {
                lift.Detach(this);
                lift.Detach(_logger);
                lift.Dispose();
            }
            _dbWorker.Dispose();
            _toolTip.Dispose();
            _shownLog?.Dispose();
        }

        // ==================================================================
        //  Task 2 - Request buttons outside the lifts (hall calls)
        //  The dispatcher sends the lift with the shortest response time.
        // ==================================================================

        private void btnCall0_Click(object sender, EventArgs e) => CallLiftToFloor(0);

        private void btnCall1_Click(object sender, EventArgs e) => CallLiftToFloor(1);

        private void CallLiftToFloor(int floor)
        {
            try
            {
                // Light first: if a lift is already open here it clears it straight away.
                _hallButtons[floor].IsLit = true;
                Elevator chosen = _dispatcher.DispatchHallCall(floor);
                _logger.Log("Hall", "Call → Lift " + chosen.Id, floor);
            }
            catch (Exception ex)
            {
                _hallButtons[floor].IsLit = false;
                ShowError("Could not call a lift", ex);
            }
        }

        // ==================================================================
        //  Task 2 - Control panels inside the lifts
        //  Requests made inside a lift are handled only by that lift.
        // ==================================================================

        private void btnAFloor0_Click(object sender, EventArgs e) => SendLiftToFloor(_liftA, 0);
        private void btnAFloor1_Click(object sender, EventArgs e) => SendLiftToFloor(_liftA, 1);
        private void btnADoorOpen_Click(object sender, EventArgs e) => OpenDoors(_liftA);
        private void btnADoorClose_Click(object sender, EventArgs e) => CloseDoors(_liftA);

        private void btnBFloor0_Click(object sender, EventArgs e) => SendLiftToFloor(_liftB, 0);
        private void btnBFloor1_Click(object sender, EventArgs e) => SendLiftToFloor(_liftB, 1);
        private void btnBDoorOpen_Click(object sender, EventArgs e) => OpenDoors(_liftB);
        private void btnBDoorClose_Click(object sender, EventArgs e) => CloseDoors(_liftB);

        private void SendLiftToFloor(Elevator lift, int floor)
        {
            try
            {
                _views[lift].FloorButtons[floor].IsLit = true;
                lift.RequestFloor(floor);
            }
            catch (Exception ex)
            {
                _views[lift].FloorButtons[floor].IsLit = false;
                ShowError("Could not send Lift " + lift.Id, ex);
            }
        }

        private void OpenDoors(Elevator lift)
        {
            if (!lift.OpenDoors())
                SystemSounds.Beep.Play(); // doors stay shut while the car is moving
        }

        private void CloseDoors(Elevator lift)
        {
            if (!lift.CloseDoors())
                SystemSounds.Beep.Play();
        }

        // ==================================================================
        //  Observer callback - keeps every display in step with the lift
        // ==================================================================

        public void OnElevatorEvent(Elevator elevator, ElevatorEventArgs e)
        {
            if (IsDisposed || !_views.ContainsKey(elevator)) return;

            if (e.Type == ElevatorEventType.DoorsOpened || e.Type == ElevatorEventType.DoorsHeldOpen)
            {
                // The request for this floor has been served.
                _views[elevator].FloorButtons[e.Floor].IsLit = false;
                _hallButtons[e.Floor].IsLit = false;
            }

            Render(elevator);
        }

        /// <summary>Task 4 - draws one animation frame for a lift.</summary>
        private void Render(Elevator lift)
        {
            LiftView view = _views[lift];

            // Car position: interpolate between the Ground (G) and Floor 1 resting points.
            int top0 = view.Shaft.GetCarTopForFloor(0, view.Car.Height);
            int top1 = view.Shaft.GetCarTopForFloor(1, view.Car.Height);
            int top = (int)Math.Round(top0 + (top1 - top0) * lift.Position);
            if (view.Car.Top != top)
                view.Car.Top = top;

            view.Car.DoorOpenRatio = lift.DoorOpenRatio;

            // All displays show the same floor and direction.
            string floorText = Elevator.FloorName(lift.DisplayFloor);
            TravelDirection arrow = ToArrow(lift.Direction);
            view.CarDisplay.DisplayText = floorText;
            view.CarDisplay.Direction = arrow;
            foreach (var hallDisplay in view.HallDisplays)
            {
                hallDisplay.DisplayText = floorText;
                hallDisplay.Direction = arrow;
            }

            string status = lift.State.Describe(lift);
            if (view.Status.Text != status)
                view.Status.Text = status;
        }

        private static TravelDirection ToArrow(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return TravelDirection.Up;
                case Direction.Down: return TravelDirection.Down;
                default: return TravelDirection.None;
            }
        }

        // ==================================================================
        //  Task 3 - Log (all database work runs on the BackgroundWorker)
        // ==================================================================

        private void btnShowLog_Click(object sender, EventArgs e)
        {
            _logVisible = true;
            RefreshLog();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            var answer = MessageBox.Show(this,
                "Delete every entry in the operation log?\nThis cannot be undone.",
                "Clear log", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (answer != DialogResult.Yes) return;

            _dbWorker.Enqueue("Clear log", _repository.ClearAll, deleted =>
            {
                SetSystemStatus($"●  LOG CLEARED  ·  {deleted} ENTRIES REMOVED", StatusOk);
                if (_logVisible) RefreshLog();
            });
        }

        private void Logger_EntrySaved(object sender, EventArgs e)
        {
            if (_logVisible) RefreshLog(); // keep the table live once it is shown
        }

        private void RefreshLog()
        {
            if (_logRefreshQueued) return; // one reload is already on its way
            _logRefreshQueued = true;

            _dbWorker.Enqueue("Load log", _repository.LoadAll, table =>
            {
                _logRefreshQueued = false;
                DataTable old = _shownLog;
                _shownLog = table;
                dgvLog.DataSource = table;
                dgvLog.ClearSelection();
                old?.Dispose();
                btnShowLog.Text = "REFRESH";
            });
        }

        /// <summary>
        /// Shows only the time in the grid (full date in the tooltip) and shows
        /// floor 0 as "G". The database keeps the plain numbers.
        /// </summary>
        private void dgvLog_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvLog.Columns[e.ColumnIndex] == colFloor && e.Value is long)
            {
                e.Value = Elevator.FloorName((int)(long)e.Value);
                e.FormattingApplied = true;
                return;
            }

            if (dgvLog.Columns[e.ColumnIndex] != colTime) return;
            string full = e.Value as string;
            if (full != null && full.Length >= 19)
            {
                e.Value = full.Substring(11, 8);
                e.FormattingApplied = true;
                dgvLog.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = full;
            }
        }

        private void DbWorker_JobFailed(string jobName, Exception ex)
        {
            if (jobName == "Load log") _logRefreshQueued = false;

            SetSystemStatus("●  DATABASE PROBLEM  ·  HOVER FOR DETAILS", StatusError);
            _toolTip.SetToolTip(lblSystemStatus, jobName + " failed: " + ex.Message);

            // Background saves only update the status line; user actions get a message box.
            if (jobName != "Save log entry")
                ShowError(jobName + " failed", ex);
        }

        // ==================================================================
        //  Helpers
        // ==================================================================

        private void SetSystemStatus(string text, Color color)
        {
            lblSystemStatus.Text = text;
            lblSystemStatus.ForeColor = color;
        }

        private void ShowError(string title, Exception ex)
        {
            MessageBox.Show(this, ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void SetupToolTips()
        {
            _toolTip.SetToolTip(btnCall0, "Call a lift to the Ground floor (G) – the nearest available lift comes");
            _toolTip.SetToolTip(btnCall1, "Call a lift to Floor 1 – the nearest available lift comes");
            foreach (var pair in _views)
            {
                string name = "Lift " + pair.Key.Id;
                _toolTip.SetToolTip(pair.Value.FloorButtons[0], "Send " + name + " to the Ground floor (G)");
                _toolTip.SetToolTip(pair.Value.FloorButtons[1], "Send " + name + " to Floor 1");
            }
            _toolTip.SetToolTip(btnADoorOpen, "Open / hold Lift A doors");
            _toolTip.SetToolTip(btnADoorClose, "Close Lift A doors now");
            _toolTip.SetToolTip(btnBDoorOpen, "Open / hold Lift B doors");
            _toolTip.SetToolTip(btnBDoorClose, "Close Lift B doors now");
            _toolTip.SetToolTip(btnShowLog, "Load the operation history from the database");
            _toolTip.SetToolTip(btnClearLog, "Delete all log entries");
        }

        /// <summary>Linen wall background with a walnut header band and brass trim.</summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var g = e.Graphics;
            Theme.PaintFabric(g, ClientRectangle, BackColor);

            var header = new Rectangle(0, 0, ClientSize.Width, 78);
            Theme.PaintWood(g, header);
            using (var shade = new System.Drawing.Drawing2D.LinearGradientBrush(header,
                       Color.FromArgb(60, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), 90f))
                g.FillRectangle(shade, header);

            using (var brass = new Pen(Theme.Brass, 3f))
                g.DrawLine(brass, 0, 79, ClientSize.Width, 79);
            using (var light = new Pen(Theme.WithAlpha(Theme.WarmLight, 140), 1f))
                g.DrawLine(light, 0, 81, ClientSize.Width, 81);
        }
    }
}
