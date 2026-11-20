using System;

namespace PremiumLift.Models
{
    /// <summary>Direction of travel of a lift car.</summary>
    public enum Direction
    {
        None,
        Up,
        Down
    }

    /// <summary>Everything an <see cref="Elevator"/> can tell its observers.</summary>
    public enum ElevatorEventType
    {
        StateChanged,
        RequestRegistered,
        Departed,
        PositionChanged,
        Arrived,
        DoorMoving,
        DoorsOpened,
        DoorsHeldOpen,
        DoorsClosed
    }

    /// <summary>Data sent with every notification.</summary>
    public sealed class ElevatorEventArgs : EventArgs
    {
        public ElevatorEventArgs(ElevatorEventType type, int floor, Direction direction)
        {
            Type = type;
            Floor = floor;
            Direction = direction;
            Timestamp = DateTime.Now;
        }

        public ElevatorEventType Type { get; }
        public int Floor { get; }
        public Direction Direction { get; }
        public DateTime Timestamp { get; }
    }

    /// <summary>
    /// OBSERVER PATTERN - anything that wants to react to a lift (the GUI,
    /// the database logger) implements this and is attached to the lift.
    /// </summary>
    public interface IElevatorObserver
    {
        void OnElevatorEvent(Elevator elevator, ElevatorEventArgs e);
    }
}
