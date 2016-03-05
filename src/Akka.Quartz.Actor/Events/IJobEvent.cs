//-----------------------------------------------------------------------
// <copyright file="IJobEvent.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2016 Typesafe Inc. <http://www.typesafe.com>
//     Copyright (C) 2013-2016 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using Quartz;

namespace Akka.Quartz.Actor.Events
{
    /// <summary>
    /// Base interface for job events
    /// </summary>
    internal interface IJobEvent
    {
        JobKey JobKey { get; }
        TriggerKey TriggerKey { get; }
    }
}