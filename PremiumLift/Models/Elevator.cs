using System;
using System.Collections.Generic;
using System.Linq;
using PremiumLift.Models.States;

namespace PremiumLift.Models
{
    /// <summary>
    /// One lift car.
    ///
    /// STATE PATTERN   - behaviour is delegated to the current
    ///                   <see cref="ElevatorState"/> (Idle, Moving, DoorsOpening,
    ///                   DoorsOpen, DoorsClosing).
    /// OBSERVER PATTERN- the lift is the subject; observers (GUI, logger)
    ///                   are attached and notified of every change.
    /// TIMER + DELEGATE- a WinForms Timer drives the animation; each tick
    ///                   invokes the current state's Tick method through the
    ///                   <see cref="AnimationStep"/> delegate (Task 4).
    /// </summary>
    public sealed class Elevator : IDisposable
    {
        public const int MinFloor = 0;
        public const int MaxFloor = 1;

        public const int TickMs = 30;                     // animation frame interval
        public const double TravelSecondsPerFloor = 2.4;  // time to travel one floor
        public const double DoorSeconds = 0.9;            // time to open OR close the doors
        public const double DwellSeconds = 2.5;           // how long doors stay open

        public static double MoveStepPerTick => TickMs / 1000.0 / TravelSecondsPerFloor;
        public static float DoorStepPerTick => (float)(TickMs / 1000.0 / DoorSeconds);
        public static int DwellTicks => (int)Math.Round(DwellSeconds * 1000.0 / TickMs);
        public static double DoorCycleSeconds => DoorSeconds + DwellSeconds + DoorSeconds;

        /// <summary>Delegate used to run one animation step (Task 4).</summary>
        public delegate void AnimationStep(Elevator elevator);

        private readonly System.Windows.Forms.Timer _timer;
        private readonly List<IElevatorObserver> _observers = new List<IElevatorObserver>();
        private readonly List<int> _requests = new List<int>();
        private ElevatorState _state;
        private bool _disposed;

        public Elevator(string id, int startFloor)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("A lift needs a name.", nameof(id));
            ValidateFloor(startFloor);

            Id = id;
            CurrentFloor = startFloor;
            Position = startFloor;
            _state = ElevatorState.Idle;

            _timer = new System.Windows.Forms.Timer { Interval = TickMs };
            _timer.Tick += Timer_Tick;
        }

        // ------------------------------------------------------------------
        //  Public read-only status
        // ------------------------------------------------------------------

        public string Id { get; }

        /// <summary>Last floor the car stopped at (or is stopped at).</summary>
        public int CurrentFloor { get; internal set; }

        /// <summary>Continuous position in floors: 0.0 = Floor 0, 1.0 = Floor 1.</summary>
        public double Position { get; internal set; }

        /// <summary>0 = doors closed, 1 = fully open.</summary>
        public float DoorOpenRatio { get; internal set; }

        public Direction Direction { get; internal set; }

        /// <summary>Floor the car is travelling to (valid while Moving).</summary>
        public int TargetFloor { get; internal set; }

        public ElevatorState State => _state;

        /// <summary>Floor to show on the displays (nearest floor while moving).</summary>
        public int DisplayFloor => (int)Math.Round(Position);

        /// <summary>
        /// How a floor is shown to people: Floor 0 is the Ground floor "G".
        /// Internally floors stay numbers (0, 1) so the logic is unchanged.
        /// </summary>
        public static string FloorName(int floor) => floor == 0 ? "G" : floor.ToString();

        public IReadOnlyList<int> PendingRequests => _requests;

        internal int DwellTicksRemaining { get; set; }

        // ------------------------------------------------------------------
        //  Observer pattern (subject side)
        // ------------------------------------------------------------------

        public void Attach(IElevatorObserver observer)
        {
            if (observer != null && !_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void Detach(IElevatorObserver observer)
        {
            _observers.Remove(observer);
        }

        internal void Notify(ElevatorEventType type, int? floor = null)
        {
            var args = new ElevatorEventArgs(type, floor ?? CurrentFloor, Direction);
            foreach (var observer in _observers.ToArray())
                observer.OnElevatorEvent(this, args);
        }

        // ------------------------------------------------------------------
        //  Commands (called by the GUI / dispatcher)
        // ------------------------------------------------------------------

        /// <summary>Ask this lift to go to a floor. Never interrupts a trip in progress.</summary>
        public void RequestFloor(int floor)
        {
            ThrowIfDisposed();
            ValidateFloor(floor);
            _state.HandleRequest(this, floor);
            StartAnimation();
        }

        /// <summary>Returns false when the doors cannot open (car is moving).</summary>
        public bool OpenDoors()
        {
            ThrowIfDisposed();
            bool accepted = _state.HandleOpenDoors(this);
            if (accepted) StartAnimation();
            return accepted;
        }

        public bool CloseDoors()
        {
            ThrowIfDisposed();
            bool accepted = _state.HandleCloseDoors(this);
            if (accepted) StartAnimation();
            return accepted;
        }

        /// <summary>Stops the animation (used when the app closes).</summary>
        public void Stop()
        {
            _timer.Stop();
        }

        // ------------------------------------------------------------------
        //  Dispatching support (Task 5.2)
        // ------------------------------------------------------------------

        /// <summary>
        /// Estimated seconds until this lift could have its doors open at
        /// <paramref name="floor"/>, finishing the current task and every
        /// queued request first, so ongoing work is never disrupted.
        /// </summary>
        public double EstimateTimeTo(int floor)
        {
            ValidateFloor(floor);

            bool reached;
            double position;
            double seconds = _state.EstimateRemainingSeconds(this, floor, out position, out reached);
            if (reached) return seconds;

            foreach (int request in _requests)
            {
                double leg = Math.Abs(request - position) * TravelSecondsPerFloor;
                if (request == floor) return seconds + leg;
                seconds += leg + DoorCycleSeconds;
                position = request;
            }
            return seconds + Math.Abs(floor - position) * TravelSecondsPerFloor;
        }

        /// <summary>True if this lift is already heading to / queued for the floor.</summary>
        public bool IsServing(int floor)
        {
            return _requests.Contains(floor) || (_state.IsMoving && TargetFloor == floor);
        }

        // ------------------------------------------------------------------
        //  Helpers used by the state classes
        // ------------------------------------------------------------------

        internal bool AddRequest(int floor)
        {
            if (_requests.Contains(floor)) return false;
            _requests.Add(floor);
            Notify(ElevatorEventType.RequestRegistered, floor);
            return true;
        }

        internal bool RemoveRequest(int floor) => _requests.Remove(floor);

        internal int? NextRequest => _requests.Count > 0 ? _requests[0] : (int?)null;

        internal void TransitionTo(ElevatorState next)
        {
            _state = next;
            next.Enter(this);
            Notify(ElevatorEventType.StateChanged);
        }

        internal void StartAnimation()
        {
            if (!_disposed && !_timer.Enabled) _timer.Start();
        }

        internal void StopAnimation() => _timer.Stop();

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Task 4: the timer drives the animation through a delegate that
            // points at the CURRENT state's Tick method.
            AnimationStep step = _state.Tick;
            step(this);
        }

        private static void ValidateFloor(int floor)
        {
            if (floor < MinFloor || floor > MaxFloor)
                throw new ArgumentOutOfRangeException(nameof(floor),
                    $"Floor must be between {MinFloor} and {MaxFloor}.");
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException("Lift " + Id);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _timer.Stop();
            _timer.Tick -= Timer_Tick;
            _timer.Dispose();
            _observers.Clear();
        }

        public override string ToString() => "Lift " + Id;
    }
}
