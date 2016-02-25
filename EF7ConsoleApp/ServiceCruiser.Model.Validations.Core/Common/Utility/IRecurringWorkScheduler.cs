﻿using System;

namespace ServiceCruiser.Model.Validations.Core.Common.Utility
{
    /// <summary>
    /// This interface represents a task that will be run at recurring intervals.
    /// </summary>
    public interface IRecurringWorkScheduler
    {
        /// <summary>
        /// Set the delegate that will be run when the schedule
        /// determines it should run.
        /// </summary>
        /// <param name="recurringWork"></param>
        void SetAction(Action recurringWork);

        /// <summary>
        /// Start the scheduler running.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the scheduler.
        /// </summary>
        void Stop();

        /// <summary>
        /// Forces the scheduler to perform the action as soon as possible, and not necessarily in a synchronous manner.
        /// </summary>
        void ForceDoWork();
    }
}
