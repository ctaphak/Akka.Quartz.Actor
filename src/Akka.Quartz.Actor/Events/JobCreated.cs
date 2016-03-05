//-----------------------------------------------------------------------
// <copyright file="JobCreated.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2016 Typesafe Inc. <http://www.typesafe.com>
//     Copyright (C) 2013-2016 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using Quartz;

namespace Akka.Quartz.Actor.Events
{
    /// <summary>
    ///     Job created event
    /// </summary>
    public class JobCreated : JobEvent
    {
        public JobCreated(JobKey jobKey, TriggerKey triggerKey) : base(jobKey, triggerKey)
        {
        }


        public override string ToString()
        {
            return string.Format("{0} with trigger {1} has been created.", JobKey, TriggerKey);
        }
    }

    /// <summary>
    ///     Job removed event
    /// </summary>
    public class JobRemoved : JobEvent
    {
        public JobRemoved(JobKey jobKey, TriggerKey triggerKey) : base(jobKey, triggerKey)
        {
        }


        public override string ToString()
        {
            return string.Format("{0} with trigger {1} has been removed.", JobKey, TriggerKey);
        }
    }
}