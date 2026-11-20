namespace PremiumLift
{
    partial class MainForm
    {
        /// <summary>Required designer variable.</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>Clean up any resources being used.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle headerStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle altRowStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblSystemStatus = new System.Windows.Forms.Label();
            this.dispHall1A = new PremiumLift.UI.DigitalDisplay();
            this.dispHall1B = new PremiumLift.UI.DigitalDisplay();
            this.btnCall1 = new PremiumLift.UI.LuxButton();
            this.lblHallHint1 = new System.Windows.Forms.Label();
            this.pnlHall1 = new PremiumLift.UI.GlassPanel();
            this.dispHall0A = new PremiumLift.UI.DigitalDisplay();
            this.dispHall0B = new PremiumLift.UI.DigitalDisplay();
            this.btnCall0 = new PremiumLift.UI.LuxButton();
            this.lblHallHint0 = new System.Windows.Forms.Label();
            this.pnlHall0 = new PremiumLift.UI.GlassPanel();
            this.carA = new PremiumLift.UI.ElevatorCar();
            this.pnlShaftA = new PremiumLift.UI.ShaftPanel();
            this.carB = new PremiumLift.UI.ElevatorCar();
            this.pnlShaftB = new PremiumLift.UI.ShaftPanel();
            this.pnlBuilding = new PremiumLift.UI.GlassPanel();
            this.dispCarA = new PremiumLift.UI.DigitalDisplay();
            this.btnAFloor1 = new PremiumLift.UI.LuxButton();
            this.btnAFloor0 = new PremiumLift.UI.LuxButton();
            this.btnADoorOpen = new PremiumLift.UI.LuxButton();
            this.btnADoorClose = new PremiumLift.UI.LuxButton();
            this.lblStatusA = new System.Windows.Forms.Label();
            this.lblPanelHintA = new System.Windows.Forms.Label();
            this.pnlControlA = new PremiumLift.UI.GlassPanel();
            this.dispCarB = new PremiumLift.UI.DigitalDisplay();
            this.btnBFloor1 = new PremiumLift.UI.LuxButton();
            this.btnBFloor0 = new PremiumLift.UI.LuxButton();
            this.btnBDoorOpen = new PremiumLift.UI.LuxButton();
            this.btnBDoorClose = new PremiumLift.UI.LuxButton();
            this.lblStatusB = new System.Windows.Forms.Label();
            this.lblPanelHintB = new System.Windows.Forms.Label();
            this.pnlControlB = new PremiumLift.UI.GlassPanel();
            this.btnShowLog = new PremiumLift.UI.LuxButton();
            this.btnClearLog = new PremiumLift.UI.LuxButton();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLift = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFloor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvLog = new System.Windows.Forms.DataGridView();
            this.pnlLog = new PremiumLift.UI.GlassPanel();
            this.pnlHall1.SuspendLayout();
            this.pnlHall0.SuspendLayout();
            this.pnlShaftA.SuspendLayout();
            this.pnlShaftB.SuspendLayout();
            this.pnlBuilding.SuspendLayout();
            this.pnlControlA.SuspendLayout();
            this.pnlControlB.SuspendLayout();
            this.pnlLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(230, 204, 150);
            this.lblTitle.Location = new System.Drawing.Point(22, 12);
            this.lblTitle.Text = "AURUM LIFT";
            this.lblTitle.Name = "lblTitle";
            //
            // lblSubtitle
            //
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(238, 230, 216);
            this.lblSubtitle.Location = new System.Drawing.Point(26, 50);
            this.lblSubtitle.Text = "Premium Dual-Elevator Control System  ·  Two-Floor Residence";
            this.lblSubtitle.Name = "lblSubtitle";
            //
            // lblSystemStatus
            //
            this.lblSystemStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblSystemStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblSystemStatus.ForeColor = System.Drawing.Color.FromArgb(123, 227, 163);
            this.lblSystemStatus.Location = new System.Drawing.Point(1092, 30);
            this.lblSystemStatus.Size = new System.Drawing.Size(300, 22);
            this.lblSystemStatus.Text = "●  SYSTEM READY  ·  2 LIFTS ONLINE";
            this.lblSystemStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSystemStatus.Name = "lblSystemStatus";
            this.lblSystemStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //
            // dispHall1A
            //
            this.dispHall1A.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.dispHall1A.Caption = "Lift A";
            this.dispHall1A.DisplayText = "G";
            this.dispHall1A.Location = new System.Drawing.Point(11, 42);
            this.dispHall1A.Size = new System.Drawing.Size(136, 44);
            this.dispHall1A.CompactRow = true;
            this.dispHall1A.Name = "dispHall1A";
            //
            // dispHall1B
            //
            this.dispHall1B.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.dispHall1B.Caption = "Lift B";
            this.dispHall1B.DisplayText = "1";
            this.dispHall1B.Location = new System.Drawing.Point(11, 92);
            this.dispHall1B.Size = new System.Drawing.Size(136, 44);
            this.dispHall1B.CompactRow = true;
            this.dispHall1B.Name = "dispHall1B";
            //
            // btnCall1
            //
            this.btnCall1.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.btnCall1.Caption = "Call";
            this.btnCall1.Font = new System.Drawing.Font("Segoe UI Symbol", 16F);
            this.btnCall1.Location = new System.Drawing.Point(44, 146);
            this.btnCall1.Size = new System.Drawing.Size(70, 70);
            this.btnCall1.TabIndex = 0;
            this.btnCall1.Text = "▼";
            this.btnCall1.Name = "btnCall1";
            this.btnCall1.Click += new System.EventHandler(this.btnCall1_Click);
            //
            // lblHallHint1
            //
            this.lblHallHint1.BackColor = System.Drawing.Color.Transparent;
            this.lblHallHint1.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblHallHint1.ForeColor = System.Drawing.Color.FromArgb(142, 137, 130);
            this.lblHallHint1.Location = new System.Drawing.Point(6, 220);
            this.lblHallHint1.Size = new System.Drawing.Size(146, 24);
            this.lblHallHint1.Text = "Sends the nearest lift";
            this.lblHallHint1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHallHint1.Name = "lblHallHint1";
            //
            // pnlHall1
            //
            this.pnlHall1.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.pnlHall1.Surface = PremiumLift.UI.PanelSurface.BlackGlass;
            this.pnlHall1.CornerRadius = 14;
            this.pnlHall1.Location = new System.Drawing.Point(16, 50);
            this.pnlHall1.Size = new System.Drawing.Size(158, 250);
            this.pnlHall1.Title = "Floor 1";
            this.pnlHall1.Controls.Add(this.dispHall1A);
            this.pnlHall1.Controls.Add(this.dispHall1B);
            this.pnlHall1.Controls.Add(this.btnCall1);
            this.pnlHall1.Controls.Add(this.lblHallHint1);
            this.pnlHall1.Name = "pnlHall1";
            //
            // dispHall0A
            //
            this.dispHall0A.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.dispHall0A.Caption = "Lift A";
            this.dispHall0A.DisplayText = "G";
            this.dispHall0A.Location = new System.Drawing.Point(11, 42);
            this.dispHall0A.Size = new System.Drawing.Size(136, 44);
            this.dispHall0A.CompactRow = true;
            this.dispHall0A.Name = "dispHall0A";
            //
            // dispHall0B
            //
            this.dispHall0B.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.dispHall0B.Caption = "Lift B";
            this.dispHall0B.DisplayText = "1";
            this.dispHall0B.Location = new System.Drawing.Point(11, 92);
            this.dispHall0B.Size = new System.Drawing.Size(136, 44);
            this.dispHall0B.CompactRow = true;
            this.dispHall0B.Name = "dispHall0B";
            //
            // btnCall0
            //
            this.btnCall0.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.btnCall0.Caption = "Call";
            this.btnCall0.Font = new System.Drawing.Font("Segoe UI Symbol", 16F);
            this.btnCall0.Location = new System.Drawing.Point(44, 146);
            this.btnCall0.Size = new System.Drawing.Size(70, 70);
            this.btnCall0.TabIndex = 1;
            this.btnCall0.Text = "▲";
            this.btnCall0.Name = "btnCall0";
            this.btnCall0.Click += new System.EventHandler(this.btnCall0_Click);
            //
            // lblHallHint0
            //
            this.lblHallHint0.BackColor = System.Drawing.Color.Transparent;
            this.lblHallHint0.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblHallHint0.ForeColor = System.Drawing.Color.FromArgb(142, 137, 130);
            this.lblHallHint0.Location = new System.Drawing.Point(6, 220);
            this.lblHallHint0.Size = new System.Drawing.Size(146, 24);
            this.lblHallHint0.Text = "Sends the nearest lift";
            this.lblHallHint0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHallHint0.Name = "lblHallHint0";
            //
            // pnlHall0
            //
            this.pnlHall0.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.pnlHall0.Surface = PremiumLift.UI.PanelSurface.BlackGlass;
            this.pnlHall0.CornerRadius = 14;
            this.pnlHall0.Location = new System.Drawing.Point(16, 367);
            this.pnlHall0.Size = new System.Drawing.Size(158, 250);
            this.pnlHall0.Title = "Ground Floor  ·  G";
            this.pnlHall0.Controls.Add(this.dispHall0A);
            this.pnlHall0.Controls.Add(this.dispHall0B);
            this.pnlHall0.Controls.Add(this.btnCall0);
            this.pnlHall0.Controls.Add(this.lblHallHint0);
            this.pnlHall0.Name = "pnlHall0";
            //
            // carA
            //
            this.carA.BackColor = System.Drawing.Color.FromArgb(24, 19, 15);
            this.carA.CarLabel = "Lift A";
            this.carA.Location = new System.Drawing.Point(15, 376);
            this.carA.Size = new System.Drawing.Size(150, 250);
            this.carA.Name = "carA";
            //
            // pnlShaftA
            //
            this.pnlShaftA.BackColor = System.Drawing.Color.FromArgb(24, 19, 15);
            this.pnlShaftA.Location = new System.Drawing.Point(190, 16);
            this.pnlShaftA.Size = new System.Drawing.Size(180, 634);
            this.pnlShaftA.Controls.Add(this.carA);
            this.pnlShaftA.Name = "pnlShaftA";
            //
            // carB
            //
            this.carB.BackColor = System.Drawing.Color.FromArgb(24, 19, 15);
            this.carB.CarLabel = "Lift B";
            this.carB.Location = new System.Drawing.Point(15, 59);
            this.carB.Size = new System.Drawing.Size(150, 250);
            this.carB.Name = "carB";
            //
            // pnlShaftB
            //
            this.pnlShaftB.BackColor = System.Drawing.Color.FromArgb(24, 19, 15);
            this.pnlShaftB.Location = new System.Drawing.Point(382, 16);
            this.pnlShaftB.Size = new System.Drawing.Size(180, 634);
            this.pnlShaftB.Controls.Add(this.carB);
            this.pnlShaftB.Name = "pnlShaftB";
            //
            // pnlBuilding
            //
            this.pnlBuilding.BackColor = System.Drawing.Color.FromArgb(238, 230, 216);
            this.pnlBuilding.Location = new System.Drawing.Point(24, 90);
            this.pnlBuilding.Size = new System.Drawing.Size(574, 666);
            this.pnlBuilding.Title = "Building";
            this.pnlBuilding.Controls.Add(this.pnlHall1);
            this.pnlBuilding.Controls.Add(this.pnlHall0);
            this.pnlBuilding.Controls.Add(this.pnlShaftA);
            this.pnlBuilding.Controls.Add(this.pnlShaftB);
            this.pnlBuilding.Name = "pnlBuilding";
            //
            // dispCarA
            //
            this.dispCarA.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.dispCarA.Caption = "Floor";
            this.dispCarA.DisplayText = "G";
            this.dispCarA.Location = new System.Drawing.Point(18, 50);
            this.dispCarA.Size = new System.Drawing.Size(174, 100);
            this.dispCarA.Name = "dispCarA";
            //
            // btnAFloor1
            //
            this.btnAFloor1.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.btnAFloor1.Font = new System.Drawing.Font("Segoe UI Light", 26F);
            this.btnAFloor1.Location = new System.Drawing.Point(60, 176);
            this.btnAFloor1.Size = new System.Drawing.Size(90, 90);
            this.btnAFloor1.TabIndex = 2;
            this.btnAFloor1.Text = "1";
            this.btnAFloor1.Name = "btnAFloor1";
            this.btnAFloor1.Click += new System.EventHandler(this.btnAFloor1_Click);
            //
            // btnAFloor0
            //
            this.btnAFloor0.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.btnAFloor0.Font = new System.Drawing.Font("Segoe UI Light", 26F);
            this.btnAFloor0.Location = new System.Drawing.Point(60, 282);
            this.btnAFloor0.Size = new System.Drawing.Size(90, 90);
            this.btnAFloor0.TabIndex = 3;
            this.btnAFloor0.Text = "G";
            this.btnAFloor0.Name = "btnAFloor0";
            this.btnAFloor0.Click += new System.EventHandler(this.btnAFloor0_Click);
            //
            // btnADoorOpen
            //
            this.btnADoorOpen.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.btnADoorOpen.Caption = "Open";
            this.btnADoorOpen.Font = new System.Drawing.Font("Segoe UI Symbol", 12F);
            this.btnADoorOpen.Location = new System.Drawing.Point(30, 404);
            this.btnADoorOpen.Size = new System.Drawing.Size(70, 70);
            this.btnADoorOpen.TabIndex = 4;
            this.btnADoorOpen.Text = "◀▶";
            this.btnADoorOpen.Name = "btnADoorOpen";
            this.btnADoorOpen.Click += new System.EventHandler(this.btnADoorOpen_Click);
            //
            // btnADoorClose
            //
            this.btnADoorClose.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.btnADoorClose.Caption = "Close";
            this.btnADoorClose.Font = new System.Drawing.Font("Segoe UI Symbol", 12F);
            this.btnADoorClose.Location = new System.Drawing.Point(110, 404);
            this.btnADoorClose.Size = new System.Drawing.Size(70, 70);
            this.btnADoorClose.TabIndex = 5;
            this.btnADoorClose.Text = "▶◀";
            this.btnADoorClose.Name = "btnADoorClose";
            this.btnADoorClose.Click += new System.EventHandler(this.btnADoorClose_Click);
            //
            // lblStatusA
            //
            this.lblStatusA.BackColor = System.Drawing.Color.Transparent;
            this.lblStatusA.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblStatusA.ForeColor = System.Drawing.Color.FromArgb(236, 232, 224);
            this.lblStatusA.Location = new System.Drawing.Point(14, 496);
            this.lblStatusA.Size = new System.Drawing.Size(182, 44);
            this.lblStatusA.Text = "IDLE  ·  DOORS CLOSED";
            this.lblStatusA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatusA.Name = "lblStatusA";
            //
            // lblPanelHintA
            //
            this.lblPanelHintA.BackColor = System.Drawing.Color.Transparent;
            this.lblPanelHintA.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblPanelHintA.ForeColor = System.Drawing.Color.FromArgb(142, 137, 130);
            this.lblPanelHintA.Location = new System.Drawing.Point(18, 560);
            this.lblPanelHintA.Size = new System.Drawing.Size(174, 80);
            this.lblPanelHintA.Text = "Buttons on this panel control Lift A only.";
            this.lblPanelHintA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPanelHintA.Name = "lblPanelHintA";
            //
            // pnlControlA
            //
            this.pnlControlA.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.pnlControlA.Surface = PremiumLift.UI.PanelSurface.BlackGlass;
            this.pnlControlA.Location = new System.Drawing.Point(618, 90);
            this.pnlControlA.Size = new System.Drawing.Size(210, 666);
            this.pnlControlA.Title = "Lift A  ·  Panel";
            this.pnlControlA.Controls.Add(this.dispCarA);
            this.pnlControlA.Controls.Add(this.btnAFloor1);
            this.pnlControlA.Controls.Add(this.btnAFloor0);
            this.pnlControlA.Controls.Add(this.btnADoorOpen);
            this.pnlControlA.Controls.Add(this.btnADoorClose);
            this.pnlControlA.Controls.Add(this.lblStatusA);
            this.pnlControlA.Controls.Add(this.lblPanelHintA);
            this.pnlControlA.Name = "pnlControlA";
            //
            // dispCarB
            //
            this.dispCarB.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.dispCarB.Caption = "Floor";
            this.dispCarB.DisplayText = "1";
            this.dispCarB.Location = new System.Drawing.Point(18, 50);
            this.dispCarB.Size = new System.Drawing.Size(174, 100);
            this.dispCarB.Name = "dispCarB";
            //
            // btnBFloor1
            //
            this.btnBFloor1.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.btnBFloor1.Font = new System.Drawing.Font("Segoe UI Light", 26F);
            this.btnBFloor1.Location = new System.Drawing.Point(60, 176);
            this.btnBFloor1.Size = new System.Drawing.Size(90, 90);
            this.btnBFloor1.TabIndex = 6;
            this.btnBFloor1.Text = "1";
            this.btnBFloor1.Name = "btnBFloor1";
            this.btnBFloor1.Click += new System.EventHandler(this.btnBFloor1_Click);
            //
            // btnBFloor0
            //
            this.btnBFloor0.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.btnBFloor0.Font = new System.Drawing.Font("Segoe UI Light", 26F);
            this.btnBFloor0.Location = new System.Drawing.Point(60, 282);
            this.btnBFloor0.Size = new System.Drawing.Size(90, 90);
            this.btnBFloor0.TabIndex = 7;
            this.btnBFloor0.Text = "G";
            this.btnBFloor0.Name = "btnBFloor0";
            this.btnBFloor0.Click += new System.EventHandler(this.btnBFloor0_Click);
            //
            // btnBDoorOpen
            //
            this.btnBDoorOpen.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.btnBDoorOpen.Caption = "Open";
            this.btnBDoorOpen.Font = new System.Drawing.Font("Segoe UI Symbol", 12F);
            this.btnBDoorOpen.Location = new System.Drawing.Point(30, 404);
            this.btnBDoorOpen.Size = new System.Drawing.Size(70, 70);
            this.btnBDoorOpen.TabIndex = 8;
            this.btnBDoorOpen.Text = "◀▶";
            this.btnBDoorOpen.Name = "btnBDoorOpen";
            this.btnBDoorOpen.Click += new System.EventHandler(this.btnBDoorOpen_Click);
            //
            // btnBDoorClose
            //
            this.btnBDoorClose.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.btnBDoorClose.Caption = "Close";
            this.btnBDoorClose.Font = new System.Drawing.Font("Segoe UI Symbol", 12F);
            this.btnBDoorClose.Location = new System.Drawing.Point(110, 404);
            this.btnBDoorClose.Size = new System.Drawing.Size(70, 70);
            this.btnBDoorClose.TabIndex = 9;
            this.btnBDoorClose.Text = "▶◀";
            this.btnBDoorClose.Name = "btnBDoorClose";
            this.btnBDoorClose.Click += new System.EventHandler(this.btnBDoorClose_Click);
            //
            // lblStatusB
            //
            this.lblStatusB.BackColor = System.Drawing.Color.Transparent;
            this.lblStatusB.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblStatusB.ForeColor = System.Drawing.Color.FromArgb(236, 232, 224);
            this.lblStatusB.Location = new System.Drawing.Point(14, 496);
            this.lblStatusB.Size = new System.Drawing.Size(182, 44);
            this.lblStatusB.Text = "IDLE  ·  DOORS CLOSED";
            this.lblStatusB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatusB.Name = "lblStatusB";
            //
            // lblPanelHintB
            //
            this.lblPanelHintB.BackColor = System.Drawing.Color.Transparent;
            this.lblPanelHintB.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblPanelHintB.ForeColor = System.Drawing.Color.FromArgb(142, 137, 130);
            this.lblPanelHintB.Location = new System.Drawing.Point(18, 560);
            this.lblPanelHintB.Size = new System.Drawing.Size(174, 80);
            this.lblPanelHintB.Text = "Buttons on this panel control Lift B only.";
            this.lblPanelHintB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPanelHintB.Name = "lblPanelHintB";
            //
            // pnlControlB
            //
            this.pnlControlB.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.pnlControlB.Surface = PremiumLift.UI.PanelSurface.BlackGlass;
            this.pnlControlB.Location = new System.Drawing.Point(842, 90);
            this.pnlControlB.Size = new System.Drawing.Size(210, 666);
            this.pnlControlB.Title = "Lift B  ·  Panel";
            this.pnlControlB.Controls.Add(this.dispCarB);
            this.pnlControlB.Controls.Add(this.btnBFloor1);
            this.pnlControlB.Controls.Add(this.btnBFloor0);
            this.pnlControlB.Controls.Add(this.btnBDoorOpen);
            this.pnlControlB.Controls.Add(this.btnBDoorClose);
            this.pnlControlB.Controls.Add(this.lblStatusB);
            this.pnlControlB.Controls.Add(this.lblPanelHintB);
            this.pnlControlB.Name = "pnlControlB";
            //
            // btnShowLog
            //
            this.btnShowLog.BackColor = System.Drawing.Color.FromArgb(238, 230, 216);
            this.btnShowLog.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnShowLog.Location = new System.Drawing.Point(18, 48);
            this.btnShowLog.Size = new System.Drawing.Size(136, 42);
            this.btnShowLog.TabIndex = 10;
            this.btnShowLog.Text = "SHOW LOG";
            this.btnShowLog.IsPrimary = true;
            this.btnShowLog.Shape = PremiumLift.UI.LuxShape.Pill;
            this.btnShowLog.Name = "btnShowLog";
            this.btnShowLog.Click += new System.EventHandler(this.btnShowLog_Click);
            //
            // btnClearLog
            //
            this.btnClearLog.BackColor = System.Drawing.Color.FromArgb(238, 230, 216);
            this.btnClearLog.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnClearLog.Location = new System.Drawing.Point(166, 48);
            this.btnClearLog.Size = new System.Drawing.Size(136, 42);
            this.btnClearLog.TabIndex = 11;
            this.btnClearLog.Text = "CLEAR";
            this.btnClearLog.Shape = PremiumLift.UI.LuxShape.Pill;
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            //
            // colId
            //
            this.colId.DataPropertyName = "LogID";
            this.colId.FillWeight = 10F;
            this.colId.HeaderText = "#";
            this.colId.ReadOnly = true;
            this.colId.Name = "colId";
            //
            // colTime
            //
            this.colTime.DataPropertyName = "LogTime";
            this.colTime.FillWeight = 28F;
            this.colTime.HeaderText = "TIME";
            this.colTime.ReadOnly = true;
            this.colTime.Name = "colTime";
            //
            // colLift
            //
            this.colLift.DataPropertyName = "Lift";
            this.colLift.FillWeight = 14F;
            this.colLift.HeaderText = "LIFT";
            this.colLift.ReadOnly = true;
            this.colLift.Name = "colLift";
            //
            // colEvent
            //
            this.colEvent.DataPropertyName = "Event";
            this.colEvent.FillWeight = 34F;
            this.colEvent.HeaderText = "EVENT";
            this.colEvent.ReadOnly = true;
            this.colEvent.Name = "colEvent";
            //
            // colFloor
            //
            this.colFloor.DataPropertyName = "Floor";
            this.colFloor.FillWeight = 14F;
            this.colFloor.HeaderText = "FLR";
            this.colFloor.ReadOnly = true;
            this.colFloor.Name = "colFloor";
            //
            // dgvLog
            //
            headerStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            headerStyle.BackColor = System.Drawing.Color.FromArgb(88, 56, 30);
            headerStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            headerStyle.ForeColor = System.Drawing.Color.FromArgb(230, 204, 150);
            headerStyle.SelectionBackColor = System.Drawing.Color.FromArgb(88, 56, 30);
            headerStyle.SelectionForeColor = System.Drawing.Color.FromArgb(230, 204, 150);
            headerStyle.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            cellStyle.BackColor = System.Drawing.Color.FromArgb(248, 243, 234);
            cellStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            cellStyle.ForeColor = System.Drawing.Color.FromArgb(43, 33, 25);
            cellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(230, 204, 150);
            cellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(43, 33, 25);
            cellStyle.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            altRowStyle.BackColor = System.Drawing.Color.FromArgb(238, 230, 216);
            this.dgvLog.AllowUserToAddRows = false;
            this.dgvLog.AllowUserToDeleteRows = false;
            this.dgvLog.AllowUserToResizeRows = false;
            this.dgvLog.AutoGenerateColumns = false;
            this.dgvLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLog.BackgroundColor = System.Drawing.Color.FromArgb(248, 243, 234);
            this.dgvLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLog.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLog.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvLog.ColumnHeadersDefaultCellStyle = headerStyle;
            this.dgvLog.ColumnHeadersHeight = 34;
            this.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLog.DefaultCellStyle = cellStyle;
            this.dgvLog.AlternatingRowsDefaultCellStyle = altRowStyle;
            this.dgvLog.EnableHeadersVisualStyles = false;
            this.dgvLog.GridColor = System.Drawing.Color.FromArgb(222, 210, 192);
            this.dgvLog.Location = new System.Drawing.Point(18, 104);
            this.dgvLog.MultiSelect = false;
            this.dgvLog.ReadOnly = true;
            this.dgvLog.RowHeadersVisible = false;
            this.dgvLog.RowTemplate.Height = 30;
            this.dgvLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvLog.Size = new System.Drawing.Size(284, 544);
            this.dgvLog.TabIndex = 12;
            this.dgvLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {this.colId, this.colTime, this.colLift, this.colEvent, this.colFloor});
            this.dgvLog.Name = "dgvLog";
            //
            // pnlLog
            //
            this.pnlLog.BackColor = System.Drawing.Color.FromArgb(238, 230, 216);
            this.pnlLog.Location = new System.Drawing.Point(1072, 90);
            this.pnlLog.Size = new System.Drawing.Size(320, 666);
            this.pnlLog.Title = "Operation Log";
            this.pnlLog.Controls.Add(this.btnShowLog);
            this.pnlLog.Controls.Add(this.btnClearLog);
            this.pnlLog.Controls.Add(this.dgvLog);
            this.pnlLog.Name = "pnlLog";
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(205, 192, 171);
            this.ClientSize = new System.Drawing.Size(1416, 780);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.lblSystemStatus);
            this.Controls.Add(this.pnlBuilding);
            this.Controls.Add(this.pnlControlA);
            this.Controls.Add(this.pnlControlB);
            this.Controls.Add(this.pnlLog);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(43, 33, 25);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Aurum Lift — Dual Elevator Control System";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.pnlLog.ResumeLayout(false);
            this.pnlControlB.ResumeLayout(false);
            this.pnlControlA.ResumeLayout(false);
            this.pnlBuilding.ResumeLayout(false);
            this.pnlShaftB.ResumeLayout(false);
            this.pnlShaftA.ResumeLayout(false);
            this.pnlHall0.ResumeLayout(false);
            this.pnlHall1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblSystemStatus;
        private PremiumLift.UI.DigitalDisplay dispHall1A;
        private PremiumLift.UI.DigitalDisplay dispHall1B;
        private PremiumLift.UI.LuxButton btnCall1;
        private System.Windows.Forms.Label lblHallHint1;
        private PremiumLift.UI.GlassPanel pnlHall1;
        private PremiumLift.UI.DigitalDisplay dispHall0A;
        private PremiumLift.UI.DigitalDisplay dispHall0B;
        private PremiumLift.UI.LuxButton btnCall0;
        private System.Windows.Forms.Label lblHallHint0;
        private PremiumLift.UI.GlassPanel pnlHall0;
        private PremiumLift.UI.ElevatorCar carA;
        private PremiumLift.UI.ShaftPanel pnlShaftA;
        private PremiumLift.UI.ElevatorCar carB;
        private PremiumLift.UI.ShaftPanel pnlShaftB;
        private PremiumLift.UI.GlassPanel pnlBuilding;
        private PremiumLift.UI.DigitalDisplay dispCarA;
        private PremiumLift.UI.LuxButton btnAFloor1;
        private PremiumLift.UI.LuxButton btnAFloor0;
        private PremiumLift.UI.LuxButton btnADoorOpen;
        private PremiumLift.UI.LuxButton btnADoorClose;
        private System.Windows.Forms.Label lblStatusA;
        private System.Windows.Forms.Label lblPanelHintA;
        private PremiumLift.UI.GlassPanel pnlControlA;
        private PremiumLift.UI.DigitalDisplay dispCarB;
        private PremiumLift.UI.LuxButton btnBFloor1;
        private PremiumLift.UI.LuxButton btnBFloor0;
        private PremiumLift.UI.LuxButton btnBDoorOpen;
        private PremiumLift.UI.LuxButton btnBDoorClose;
        private System.Windows.Forms.Label lblStatusB;
        private System.Windows.Forms.Label lblPanelHintB;
        private PremiumLift.UI.GlassPanel pnlControlB;
        private PremiumLift.UI.LuxButton btnShowLog;
        private PremiumLift.UI.LuxButton btnClearLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLift;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFloor;
        private System.Windows.Forms.DataGridView dgvLog;
        private PremiumLift.UI.GlassPanel pnlLog;
    }
}
