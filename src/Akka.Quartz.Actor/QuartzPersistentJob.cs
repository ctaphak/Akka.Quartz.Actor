using Akka.Actor;
using Akka.Util.Internal;
using Quartz;

namespace Akka.Quartz.Actor
{
    /// <summary>
    /// Persistent Job
    /// </summary>
    public class QuartzPersistentJob : IJob
    {
        private const string MessageKey = "message";
        private const string ActorKey = "actor";
        public const string SysKey = "sys";

        public void Execute(IJobExecutionContext context)
        {
            var jdm = context.JobDetail.JobDataMap;
            if (jdm.ContainsKey(MessageKey) && jdm.ContainsKey(ActorKey))
            {
                var actor = jdm[ActorKey] as ActorPath;
                var sys = context.Scheduler.Context[SysKey] as ActorSystem;

                if (actor != null && sys != null)
                {
                    ActorSelection selection = sys.ActorSelection(actor);

                    selection.Tell(jdm[MessageKey]);
                }
            }
        }

        public static JobBuilder CreateBuilderWithData(ActorPath actorPath, object message)
        {
            var jdm = new JobDataMap();
            jdm.AddAndReturn(MessageKey, message).Add(ActorKey, actorPath);
            return JobBuilder.Create<QuartzPersistentJob>().UsingJobData(jdm);
        }
    }
}