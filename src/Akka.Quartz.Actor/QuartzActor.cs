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

        public QuartzActor()
        {
            _scheduler = new StdSchedulerFactory().GetScheduler();
        }

        public QuartzActor(NameValueCollection props)
        {
            _scheduler = new StdSchedulerFactory(props).GetScheduler();
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
            if (createJob.To == null)
            {
                Context.Sender.Tell(new CreateJobFail(null, null, new ArgumentNullException("createJob.To")));
            }
            if (createJob.Trigger == null)
            {
                Context.Sender.Tell(new CreateJobFail(null, null, new ArgumentNullException("createJob.Trigger")));
            }
            else
            {

                try
                {
                    var job =
                   QuartzJob.CreateBuilderWithData(createJob.To, createJob.Message)
                       .WithIdentity(createJob.Trigger.JobKey)
                       .Build();
                    _scheduler.ScheduleJob(job, createJob.Trigger);

                    Context.Sender.Tell(new JobCreated(createJob.Trigger.JobKey, createJob.Trigger.Key));
                }
                catch (Exception ex)
                {
                    Context.Sender.Tell(new CreateJobFail(createJob.Trigger.JobKey, createJob.Trigger.Key, ex));
                }
            }
        }

        protected virtual void RemoveJobCommand(RemoveJob removeJob)
        {
            try
            {
                var deleted = _scheduler.DeleteJob(removeJob.JobKey);
                if (deleted)
                {
                    Context.Sender.Tell(new JobRemoved(removeJob.JobKey, removeJob.TriggerKey));
                }
                else
                {
                    Context.Sender.Tell(new RemoveJobFail(removeJob.JobKey, removeJob.TriggerKey, new Exception("job not found")));
                }
            }
            catch (Exception ex)
            {
                Context.Sender.Tell(new RemoveJobFail(removeJob.JobKey, removeJob.TriggerKey, ex));
            }
        }
    }
}