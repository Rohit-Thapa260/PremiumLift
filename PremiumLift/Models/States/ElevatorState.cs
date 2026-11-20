namespace PremiumLift.Models.States
{
    /// <summary>
    /// STATE PATTERN - base class for every lift state. Each concrete state
    /// decides how the lift reacts to requests, door buttons and timer ticks.
    /// States hold no data of their own, so one shared instance of each is used.
    /// </summary>
    public abstract class ElevatorState
    {
        public static readonly ElevatorState Idle = new IdleState();
        public static readonly ElevatorState Moving = new MovingState();
        public static readonly ElevatorState DoorsOpening = new DoorsOpeningState();
        public static readonly ElevatorState DoorsOpen = new DoorsOpenState();
        public static readonly ElevatorState DoorsClosing = new DoorsClosingState();

        public abstract string Name { get; }

        public virtual bool IsMoving => false;

        /// <summary>Called once when the lift switches into this state.</summary>
        public virtual void Enter(Elevator elevator) { }

        /// <summary>One animation frame (called by the timer through a delegate).</summary>
        public abstract void Tick(Elevator elevator);

        /// <summary>Default: queue the request; it is served after the current task.</summary>
        public virtual void HandleRequest(Elevator elevator, int floor)
        {
            elevator.AddRequest(floor);
        }

        public virtual bool HandleOpenDoors(Elevator elevator) => false;

        public virtual bool HandleCloseDoors(Elevator elevator) => false;

        /// <summary>
        /// Seconds needed to finish what the lift is doing now.
        /// <paramref name="reachesFloor"/> is true if finishing the current task
        /// already serves <paramref name="floor"/>.
        /// </summary>
        public abstract double EstimateRemainingSeconds(Elevator elevator, int floor,
                                                        out double positionAfter, out bool reachesFloor);

        /// <summary>Short, friendly text for the status line.</summary>
        public abstract string Describe(Elevator elevator);

        public override string ToString() => Name;
    }
}
