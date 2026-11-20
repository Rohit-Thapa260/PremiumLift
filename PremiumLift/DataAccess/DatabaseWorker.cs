using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PremiumLift.DataAccess
{
    /// <summary>
    /// Task 5.1 - runs database jobs on a BackgroundWorker so the GUI and the
    /// lift animation never freeze while SQLite is working.
    /// Jobs run one at a time, in the order they were queued. Results and
    /// errors come back on the GUI thread (RunWorkerCompleted).
    /// </summary>
    public sealed class DatabaseWorker : IDisposable
    {
        private sealed class Job
        {
            public string Name;
            public Func<object> Work;
            public Action<object> OnSuccess;
        }

        private readonly Queue<Job> _queue = new Queue<Job>();
        private readonly BackgroundWorker _worker = new BackgroundWorker();
        private Job _current;
        private bool _disposed;

        public DatabaseWorker()
        {
            _worker.DoWork += Worker_DoWork;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        /// <summary>Raised on the GUI thread when a job throws.</summary>
        public event Action<string, Exception> JobFailed;

        /// <summary>Raised on the GUI thread when the queue becomes empty.</summary>
        public event EventHandler Drained;

        public bool IsBusy => _current != null || _queue.Count > 0;

        /// <summary>Queue a job. <paramref name="onSuccess"/> runs on the GUI thread.</summary>
        public void Enqueue<T>(string name, Func<T> work, Action<T> onSuccess = null)
        {
            if (_disposed) return;
            if (work == null) throw new ArgumentNullException(nameof(work));

            _queue.Enqueue(new Job
            {
                Name = name,
                Work = () => work(),
                OnSuccess = onSuccess == null ? (Action<object>)null : result => onSuccess((T)result)
            });
            StartNext();
        }

        private void StartNext()
        {
            if (_disposed || _current != null || _worker.IsBusy || _queue.Count == 0) return;
            _current = _queue.Dequeue();
            _worker.RunWorkerAsync(_current);
        }

        // Runs on a background thread.
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var job = (Job)e.Argument;
            e.Result = job.Work();
        }

        // Runs back on the GUI thread.
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Job job = _current;
            _current = null;

            if (e.Error != null)
            {
                JobFailed?.Invoke(job.Name, e.Error);
            }
            else
            {
                try
                {
                    job.OnSuccess?.Invoke(e.Result);
                }
                catch (Exception ex)
                {
                    JobFailed?.Invoke(job.Name, ex);
                }
            }

            if (_queue.Count > 0) StartNext();
            else Drained?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _queue.Clear();
            _worker.DoWork -= Worker_DoWork;
            _worker.RunWorkerCompleted -= Worker_RunWorkerCompleted;
            _worker.Dispose();
        }
    }
}
