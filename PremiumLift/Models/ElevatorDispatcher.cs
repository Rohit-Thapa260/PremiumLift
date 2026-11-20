using System;
using System.Collections.Generic;
using System.Linq;

namespace PremiumLift.Models
{
    /// <summary>
    /// Handles calls made from OUTSIDE the lifts (Task 5.2): the call goes to
    /// the lift with the shortest estimated response time. Requests are only
    /// queued, so a lift's current trip is never interrupted.
    /// Calls made INSIDE a lift go straight to that lift and never come here.
    /// </summary>
    public sealed class ElevatorDispatcher
    {
        private readonly List<Elevator> _elevators;

        public ElevatorDispatcher(params Elevator[] elevators)
        {
            if (elevators == null || elevators.Length == 0)
                throw new ArgumentException("At least one lift is required.", nameof(elevators));
            _elevators = elevators.ToList();
        }

        public IReadOnlyList<Elevator> Elevators => _elevators;

        /// <summary>Picks the lift that can reach <paramref name="floor"/> soonest.</summary>
        public Elevator SelectBest(int floor)
        {
            Elevator best = null;
            double bestSeconds = double.MaxValue;

            foreach (var elevator in _elevators)
            {
                double seconds = elevator.EstimateTimeTo(floor);
                // Small tolerance so ties go to the first lift (Lift A).
                if (seconds < bestSeconds - 0.001)
                {
                    best = elevator;
                    bestSeconds = seconds;
                }
            }
            return best;
        }

        /// <summary>
        /// Sends a lift for a hall call. If a lift is already on its way to
        /// that floor, the call is given to it rather than sending a second car.
        /// </summary>
        public Elevator DispatchHallCall(int floor)
        {
            Elevator chosen = _elevators.FirstOrDefault(e => e.IsServing(floor)) ?? SelectBest(floor);
            chosen.RequestFloor(floor);
            return chosen;
        }
    }
}
