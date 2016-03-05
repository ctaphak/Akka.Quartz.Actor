//-----------------------------------------------------------------------
// <copyright file="CreateJobFail.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2016 Typesafe Inc. <http://www.typesafe.com>
//     Copyright (C) 2013-2016 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System;
using Quartz;

namespace Akka.Quartz.Actor.Events
{
    /// <summary>
    ///     Create job fail event
    /// </summary>
    public class CreateJobFail : JobEvent
    {
        public CreateJobFail(JobKey jobKey, TriggerKey triggerKey, Exception reason) : base(jobKey, triggerKey)
        {
            Reason = reason;
        }

        /// <summary>
        ///     Fail reason
        /// </summary>
        public Exception Reason { get; }

        public override string ToString()
        {
            return string.Format("Create job {0} with trigger {1} fail. With reason {2}", JobKey, TriggerKey, Reason);
        }
    }
}