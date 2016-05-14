using System;
using System.Threading;
using Akka.Actor;
using Akka.Quartz.Actor.Commands;
using Akka.Quartz.Actor.Events;
using Xunit;

namespace Akka.Quartz.Actor.Tests
{
    public class QuartzActorSpec : TestKit.Xunit2.TestKit
    {
        [Fact]
        public void QuartzActor_Should_Create_Job()
        {
            var probe = CreateTestProbe(Sys);
            var quartzActor = Sys.ActorOf(Props.Create(() => new QuartzActor(null)), "QuartzActor");
            quartzActor.Tell(new CreateJob(probe, "Hello", " * * * * * ?"));
            ExpectMsg<JobCreated>();
            probe.ExpectMsg("Hello", TimeSpan.FromSeconds(5));
            probe.ExpectMsg("Hello", TimeSpan.FromSeconds(65));
        }

        [Fact]
        public void QuartzActor_Should_Remove_Job()
        {
            var probe = CreateTestProbe(Sys);
            var quartzActor = Sys.ActorOf(Props.Create(() => new QuartzActor(null)), "QuartzActor");
            quartzActor.Tell(new CreateJob(probe, "Hello", " * * * * * ?"));
            var jobCreated = ExpectMsg<JobCreated>();
            probe.ExpectMsg("Hello", TimeSpan.FromSeconds(5));
            quartzActor.Tell(new RemoveJob(jobCreated.JobKey, jobCreated.TriggerKey));
            ExpectMsg<JobRemoved>();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            Within(TimeSpan.FromSeconds(70), () => { probe.ExpectNoMsg(); });
        }

        [Fact]
        public void QuartzActor_Should_Fail_With_Invalid_CronString()
        {
            var probe = CreateTestProbe(Sys);
            var quartzActor = Sys.ActorOf(Props.Create(() => new QuartzActor(null)), "QuartzActor");
            quartzActor.Tell(new CreateJob(probe, "Hello", " invalid strings ?"));
            var failedJob = ExpectMsg<CreateJobFail>();
            Assert.NotNull(failedJob.Reason);
        }

        [Fact]
        public void QuartzActor_Should_Fail_With_Null_Actor()
        {
            var quartzActor = Sys.ActorOf(Props.Create(() => new QuartzActor(null)), "QuartzActor");
            quartzActor.Tell(new CreateJob(null, "Hello", " invalid strings ?"));
            var failedJob = ExpectMsg<CreateJobFail>();
            Assert.NotNull(failedJob.Reason);
        }
    }
}