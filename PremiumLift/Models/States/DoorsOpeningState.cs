namespace PremiumLift.Models.States
{
    /// <summary>Doors sliding open at the current floor.</summary>
    public sealed class DoorsOpeningState : ElevatorState
    {
        public override string Name => "Doors opening";

        public override void Enter(Elevator elevator)
        {
            elevator.Direction = Direction.None;
        }

        public override void Tick(Elevator elevator)
        {
            elevator.DoorOpenRatio += Elevator.DoorStepPerTick;

            if (elevator.DoorOpenRatio < 1f)
            {
                elevator.Notify(ElevatorEventType.DoorMoving);
                return;
            }

            elevator.DoorOpenRatio = 1f;
            elevator.RemoveRequest(elevator.CurrentFloor); // this floor is now served
            elevator.TransitionTo(DoorsOpen);
            elevator.Notify(ElevatorEventType.DoorsOpened, elevator.CurrentFloor);
        }

        public override void HandleRequest(Elevator elevator, int floor)
        {
            if (floor == elevator.CurrentFloor) return; // doors are already opening here
            base.HandleRequest(elevator, floor);
        }

        public override bool HandleOpenDoors(Elevator elevator) => true;

        public override bool HandleCloseDoors(Elevator elevator)
        {
            elevator.TransitionTo(DoorsClosing);
            return true;
        }

        public override double EstimateRemainingSeconds(Elevator elevator, int floor,
                                                        out double positionAfter, out bool reachesFloor)
        {
            double openRemaining = (1 - elevator.DoorOpenRatio) * Elevator.DoorSeconds;
            positionAfter = elevator.Position;
            reachesFloor = floor == elevator.CurrentFloor;
            return reachesFloor
                ? openRemaining
                : openRemaining + Elevator.DwellSeconds + Elevator.DoorSeconds;
        }

        public override string Describe(Elevator elevator) =>
            "DOORS OPENING  ·  FLOOR " + Elevator.FloorName(elevator.CurrentFloor);
    }
}
