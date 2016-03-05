//-----------------------------------------------------------------------
// <copyright file="QuartzActor.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2016 Typesafe Inc. <http://www.typesafe.com>
//     Copyright (C) 2013-2016 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Specialized;
using Akka.Actor;
using Akka.Quartz.Actor.Commands;
using Akka.Quartz.Actor.Events;
using Quartz;
using Quartz.Impl;
using IScheduler = Quartz.IScheduler;

namespace Akka.Quartz.Actor
{
    /// <summary>
    /// The base quartz scheduling actor. Handles a single quartz scheduler
    /// and processes Add and Remove messages.
    /// </summary>
    public class QuartzActor : ActorBase
    {
        private readonly IScheduler _scheduler;

        public QuartzActor(NameValueCollection props = null)
        {
            _scheduler = props == null
                ? new StdSchedulerFactory().GetScheduler()
                : new StdSchedulerFactory(props).GetScheduler();
        }

        protected override bool Receive(object message)
        {
            return message.Match().With<CreateJob>(CreateJobCommand).With<RemoveJob>(RemoveJobCommand).WasHandled;
        }

        protected override void PreStart()
        {
            _scheduler.Start();
            base.PreStart();
        }

        protected override void PostStop()
        {
            _scheduler.Shutdown();
            base.PostStop();
        }

        protected virtual void CreateJobCommand(CreateJob createJob)
        {
            var jobKey = CreateJobKey(createJob);
            var triggerKey = CreateTriggerKey(createJob);
            var job = QuartzJob.CreateBuilderWithData(createJob.To, createJob.Message).WithIdentity(jobKey).Build();
            try
            {
                _scheduler.ScheduleJob(job, TriggerBuilder.Create().StartNow()
                    .WithIdentity(triggerKey).ForJob(job)
                    .WithSchedule(CronScheduleBuilder.CronSchedule(createJob.Cron)).Build());

                Context.Sender.Tell(new JobCreated(jobKey, triggerKey));
            }
            catch (Exception ex)
            {
                Context.Sender.Tell(new CreateJobFail(jobKey, triggerKey, ex));
            }
        }

        protected virtual void RemoveJobCommand(RemoveJob removeJob)
        {
            try
            {
                _scheduler.DeleteJob(removeJob.JobKey);
                Context.Sender.Tell(new JobRemoved(removeJob.JobKey, removeJob.TriggerKey));
            }
            catch (Exception ex)
            {
                Context.Sender.Tell(new RemoveJobFail(removeJob.JobKey, removeJob.TriggerKey, ex));
            }
        }

        protected virtual JobKey CreateJobKey(CreateJob createJob)
        {
            return new JobKey(
                string.Format("{0}{1}{2}job", createJob.To, createJob.Message, createJob.Cron).GetHashCode().ToString());
        }

        protected virtual TriggerKey CreateTriggerKey(CreateJob createJob)
        {
            return new TriggerKey(
                string.Format("{0}{1}{2}trigger", createJob.To, createJob.Message, createJob.Cron)
                    .GetHashCode()
                    .ToString());
        }
    }
}