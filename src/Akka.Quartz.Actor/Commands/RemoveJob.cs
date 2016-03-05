//-----------------------------------------------------------------------
// <copyright file="RemoveJob.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2016 Typesafe Inc. <http://www.typesafe.com>
//     Copyright (C) 2013-2016 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------
using Quartz;

namespace Akka.Quartz.Actor.Commands
{
    /// <summary>
    ///     Message to remove a cron scheduler.
    /// </summary>
    public class RemoveJob : IJobCommand
    {
        public RemoveJob(JobKey jobKey, TriggerKey triggerKey)
        {
            JobKey = jobKey;
            TriggerKey = triggerKey;
        }

        /// <summary>
        ///     Job key
        /// </summary>
        public JobKey JobKey { get; private set; }

        /// <summary>
        ///     Trigger key
        /// </summary>
        public TriggerKey TriggerKey { get; private set; }
    }
}