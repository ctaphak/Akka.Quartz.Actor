//-----------------------------------------------------------------------
// <copyright file="CreateJob.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2016 Typesafe Inc. <http://www.typesafe.com>
//     Copyright (C) 2013-2016 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using Akka.Actor;

namespace Akka.Quartz.Actor.Commands
{
    /// <summary>
    ///     Message to add a cron scheduler.
    /// </summary>
    public class CreateJob : IJobCommand
    {
        public CreateJob(IActorRef to, object message, string cron)
        {
            To = to;
            Message = message;
            Cron = cron;
        }

        /// <summary>
        ///     The desination actor
        /// </summary>
        public IActorRef To { get; private set; }

        /// <summary>
        ///     Message
        /// </summary>
        public object Message { get; private set; }

        /// <summary>
        ///     Cron expression
        /// </summary>
        public string Cron { get; private set; }
    }
}