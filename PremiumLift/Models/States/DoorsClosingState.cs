namespace PremiumLift.Models.States
{
    /// <summary>Doors sliding shut. A call to this floor re-opens them.</summary>
    public sealed class DoorsClosingState : ElevatorState
    {
        public override string Name => "Doors closing";

        public override void Tick(Elevator elevator)
        {
            elevator.DoorOpenRatio -= Elevator.DoorStepPerTick;

            if (elevator.DoorOpenRatio > 0f)
            {
                elevator.Notify(ElevatorEventType.DoorMoving);
                return;
            }

            elevator.DoorOpenRatio = 0f;
            elevator.Notify(ElevatorEventType.DoorsClosed, elevator.CurrentFloor);
            elevator.TransitionTo(Idle); // Idle picks up the next queued request
        }

        public override void HandleRequest(Elevator elevator, int floor)
        {
            if (floor == elevator.CurrentFloor)
            {
                elevator.TransitionTo(DoorsOpening); // re-open for the new passenger
                return;
            }
            base.HandleRequest(elevator, floor);
        }

        public override bool HandleOpenDoors(Elevator elevator)
        {
            elevator.TransitionTo(DoorsOpening);
            return true;
        }

        public override bool HandleCloseDoors(Elevator elevator) => true;

        public override double EstimateRemainingSeconds(Elevator elevator, int floor,
                                                        out double positionAfter, out bool reachesFloor)
        {
            positionAfter = elevator.Position;
            reachesFloor = floor == elevator.CurrentFloor;
            double closeRemaining = elevator.DoorOpenRatio * Elevator.DoorSeconds;
            // Same floor: re-opening takes as long as it has already closed.
            return reachesFloor ? (1 - elevator.DoorOpenRatio) * Elevator.DoorSeconds : closeRemaining;
        }

        public override string Describe(Elevator elevator) =>
            "DOORS CLOSING  ·  FLOOR " + Elevator.FloorName(elevator.CurrentFloor);
    }
}
