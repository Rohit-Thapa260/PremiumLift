using System;
using System.Data;
using System.Globalization;
using PremiumLift.Models;

namespace PremiumLift.DataAccess
{
    /// <summary>
    /// OBSERVER - attached to each lift. Turns important lift events into log
    /// rows and saves them in the background through <see cref="DatabaseWorker"/>.
    /// </summary>
    public sealed class ElevatorLogger : IElevatorObserver
    {
        public const string TimeFormat = "yyyy-MM-dd HH:mm:ss";

        private readonly ElevatorLogRepository _repository;
        private readonly DatabaseWorker _worker;

        public ElevatorLogger(ElevatorLogRepository repository, DatabaseWorker worker)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _worker = worker ?? throw new ArgumentNullException(nameof(worker));
        }

        /// <summary>Raised on the GUI thread after an entry is stored.</summary>
        public event EventHandler EntrySaved;

        public void OnElevatorEvent(Elevator elevator, ElevatorEventArgs e)
        {
            string text = Describe(e);
            if (text != null)
                Log(elevator.Id, text, e.Floor, e.Timestamp);
        }

        /// <summary>Maps lift events to log text; frame-by-frame events are not logged.</summary>
        private static string Describe(ElevatorEventArgs e)
        {
            switch (e.Type)
            {
                case ElevatorEventType.RequestRegistered: return "Request " + Elevator.FloorName(e.Floor);
                case ElevatorEventType.Departed: return e.Direction == Direction.Up ? "Moving up" : "Moving down";
                case ElevatorEventType.Arrived: return "Arrived";
                case ElevatorEventType.DoorsOpened: return "Doors opened";
                case ElevatorEventType.DoorsHeldOpen: return "Doors held open";
                case ElevatorEventType.DoorsClosed: return "Doors closed";
                default: return null;
            }
        }

        /// <summary>Adds one entry (also used for hall calls and system messages).</summary>
        public void Log(string source, string message, int? floor, DateTime? when = null)
        {
            DataTable rows = ElevatorLogRepository.CreateLogTable();
            DataRow row = rows.NewRow();
            row["LogTime"] = (when ?? DateTime.Now).ToString(TimeFormat, CultureInfo.InvariantCulture);
            row["Lift"] = source;
            row["Event"] = message;
            row["Floor"] = floor.HasValue ? (object)(long)floor.Value : DBNull.Value;
            rows.Rows.Add(row); // RowState = Added -> inserted by Update()

            _worker.Enqueue("Save log entry", () => _repository.Save(rows),
                            saved => EntrySaved?.Invoke(this, EventArgs.Empty));
        }
    }
}
