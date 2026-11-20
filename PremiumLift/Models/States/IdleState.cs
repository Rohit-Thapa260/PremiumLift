namespace PremiumLift.Models.States
{
    /// <summary>Parked with doors closed, waiting for the next request.</summary>
    public sealed class IdleState : ElevatorState
    {
        public override string Name => "Idle";

        public override void Enter(Elevator elevator)
        {
            elevator.Direction = Direction.None;
            if (elevator.NextRequest == null)
                elevator.StopAnimation();
        }

        public override void Tick(Elevator elevator)
        {
            int? next = elevator.NextRequest;
            if (next == null)
            {
                elevator.StopAnimation();
                return;
            }

            int target = next.Value;
            elevator.RemoveRequest(target);

            if (target == elevator.CurrentFloor)
            {
                // Already here - just open the doors.
                elevator.TransitionTo(DoorsOpening);
                return;
            }

            elevator.TargetFloor = target;
            elevator.Direction = target > elevator.CurrentFloor ? Direction.Up : Direction.Down;
            elevator.Notify(ElevatorEventType.Departed, elevator.CurrentFloor);
            elevator.TransitionTo(Moving);
        }

        public override bool HandleOpenDoors(Elevator elevator)
        {
            elevator.TransitionTo(DoorsOpening);
            return true;
        }

        public override bool HandleCloseDoors(Elevator elevator) => true; // already closed

        public override double EstimateRemainingSeconds(Elevator elevator, int floor,
                                                        out double positionAfter, out bool reachesFloor)
        {
            positionAfter = elevator.Position;
            reachesFloor = floor == elevator.CurrentFloor;
            return 0;
        }

        public override string Describe(Elevator elevator) => "IDLE  ·  DOORS CLOSED";
    }
}
