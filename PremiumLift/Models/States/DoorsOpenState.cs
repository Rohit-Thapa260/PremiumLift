namespace PremiumLift.Models.States
{
    /// <summary>Doors fully open; they close automatically after the dwell time.</summary>
    public sealed class DoorsOpenState : ElevatorState
    {
        public override string Name => "Doors open";

        public override void Enter(Elevator elevator)
        {
            elevator.DwellTicksRemaining = Elevator.DwellTicks;
        }

        public override void Tick(Elevator elevator)
        {
            elevator.DwellTicksRemaining--;
            if (elevator.DwellTicksRemaining <= 0)
                elevator.TransitionTo(DoorsClosing);
        }

        public override void HandleRequest(Elevator elevator, int floor)
        {
            if (floor == elevator.CurrentFloor)
            {
                // Someone called the lift to where it already stands: hold the doors.
                elevator.DwellTicksRemaining = Elevator.DwellTicks;
                elevator.Notify(ElevatorEventType.DoorsHeldOpen, floor);
                return;
            }
            base.HandleRequest(elevator, floor);
        }

        public override bool HandleOpenDoors(Elevator elevator)
        {
            elevator.DwellTicksRemaining = Elevator.DwellTicks; // keep open a little longer
            return true;
        }

        public override bool HandleCloseDoors(Elevator elevator)
        {
            elevator.TransitionTo(DoorsClosing);
            return true;
        }

        public override double EstimateRemainingSeconds(Elevator elevator, int floor,
                                                        out double positionAfter, out bool reachesFloor)
        {
            positionAfter = elevator.Position;
            reachesFloor = floor == elevator.CurrentFloor;
            if (reachesFloor) return 0;
            return elevator.DwellTicksRemaining * Elevator.TickMs / 1000.0 + Elevator.DoorSeconds;
        }

        public override string Describe(Elevator elevator) =>
            "AT FLOOR " + Elevator.FloorName(elevator.CurrentFloor) + "  ·  DOORS OPEN";
    }
}
