using System;

namespace PremiumLift.Models.States
{
    /// <summary>Travelling between floors. Doors are locked shut.</summary>
    public sealed class MovingState : ElevatorState
    {
        public override string Name => "Moving";

        public override bool IsMoving => true;

        public override void Tick(Elevator elevator)
        {
            double step = Elevator.MoveStepPerTick;
            bool goingUp = elevator.Direction == Direction.Up;
            elevator.Position += goingUp ? step : -step;

            bool arrived = goingUp
                ? elevator.Position >= elevator.TargetFloor
                : elevator.Position <= elevator.TargetFloor;

            if (!arrived)
            {
                elevator.Notify(ElevatorEventType.PositionChanged);
                return;
            }

            elevator.Position = elevator.TargetFloor;
            elevator.CurrentFloor = elevator.TargetFloor;
            elevator.Notify(ElevatorEventType.Arrived, elevator.CurrentFloor);
            elevator.Direction = Direction.None;
            elevator.TransitionTo(DoorsOpening);
        }

        public override void HandleRequest(Elevator elevator, int floor)
        {
            if (floor == elevator.TargetFloor) return; // already going there
            base.HandleRequest(elevator, floor);       // queued - trip is not interrupted
        }

        // Doors cannot be opened or closed while travelling.
        public override bool HandleOpenDoors(Elevator elevator) => false;

        public override double EstimateRemainingSeconds(Elevator elevator, int floor,
                                                        out double positionAfter, out bool reachesFloor)
        {
            double travel = Math.Abs(elevator.TargetFloor - elevator.Position) * Elevator.TravelSecondsPerFloor;
            positionAfter = elevator.TargetFloor;
            reachesFloor = floor == elevator.TargetFloor;
            return reachesFloor ? travel : travel + Elevator.DoorCycleSeconds;
        }

        public override string Describe(Elevator elevator) =>
            (elevator.Direction == Direction.Up ? "MOVING UP" : "MOVING DOWN") +
            "  ·  TO FLOOR " + Elevator.FloorName(elevator.TargetFloor);
    }
}
