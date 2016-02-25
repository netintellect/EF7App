using System;
using System.Threading;

namespace ServiceCruiser.Model.Validations.Core.Common.Utility
{
    /// <summary>
    /// Scheduler that wraps a timer.
    /// </summary>
    public sealed class RecurringWorkScheduler : IRecurringWorkScheduler, IDisposable
    {
        private Timer _timer;
        private readonly TimeSpan _pollInterval;
        private Action _recurringWork;
        private bool _started;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecurringWorkScheduler"/> class.
        /// </summary>
        /// <param name="pollInterval">The poll interval.</param>
        public RecurringWorkScheduler(TimeSpan pollInterval)
        {
            if (pollInterval <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("pollInterval");

            _timer = new Timer(OnTimerIntervalElapsed, null, 0, 0);
            _pollInterval = pollInterval;
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_timer != null)
            {
                Stop();
                _timer.Dispose();
                _timer = null;
            }
        }

        /// <summary>
        /// Set the delegate that will be run when the schedule
        /// determines it should run.
        /// </summary>
        /// <param name="recurringWork"></param>
        public void SetAction(Action recurringWork)
        {
            _recurringWork = recurringWork;
        }

        /// <summary>
        /// Start the scheduler running.
        /// </summary>
        public void Start()
        {
            if (_timer != null)
            {
                _started = true;
                _timer.Change(_pollInterval, TimeSpan.FromMilliseconds(-1));
            }
        }

        /// <summary>
        /// Stop the scheduler.
        /// </summary>
        public void Stop()
        {
            _started = false;
            if (_timer != null)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        /// <summary>
        /// Forces the scheduler to perform the action as soon as possible, and not necessarily in a synchronous manner.
        /// </summary>
        public void ForceDoWork()
        {
            if (_timer != null)
            {
                _timer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(-1));
            }
        }
        
        private void OnTimerIntervalElapsed(object state)
        {
            if (_timer != null)
            {
                _recurringWork.Invoke();
                if (_started)
                {
                    Start();
                }
            }
        }
    }
}
