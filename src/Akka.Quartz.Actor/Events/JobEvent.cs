//-----------------------------------------------------------------------
// <copyright file="JobEvent.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2016 Typesafe Inc. <http://www.typesafe.com>
//     Copyright (C) 2013-2016 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------
using Quartz;

namespace Akka.Quartz.Actor.Events
{
    public abstract class JobEvent : IJobEvent
    {
        protected JobEvent(JobKey jobKey, TriggerKey triggerKey)
        {
            JobKey = jobKey;
            TriggerKey = triggerKey;
        }

        /// <summary>
        ///     Job key
        /// </summary>
        public JobKey JobKey { get; }

        /// <summary>
        ///     Trigger key
        /// </summary>
        public TriggerKey TriggerKey { get; }
    }
}