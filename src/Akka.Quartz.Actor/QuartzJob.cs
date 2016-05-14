using Akka.Actor;
using Akka.Util.Internal;
using Quartz;

namespace Akka.Quartz.Actor
{
    /// <summary>
    /// Job
    /// </summary>
    public class QuartzJob : IJob
    {
        private const string MessageKey = "message";
        private const string ActorKey = "actor";

        public void Execute(IJobExecutionContext context)
        {
            var jdm = context.JobDetail.JobDataMap;
            if (jdm.ContainsKey(MessageKey) && jdm.ContainsKey(ActorKey))
            {
                var actor = jdm[ActorKey] as IActorRef;
                if (actor != null)
                {
                    actor.Tell(jdm[MessageKey]);
                }
            }
        }

        public static JobBuilder CreateBuilderWithData(IActorRef actorRef, object message)
        {
            var jdm = new JobDataMap();
            jdm.AddAndReturn(MessageKey, message).Add(ActorKey, actorRef);
            return JobBuilder.Create<QuartzJob>().UsingJobData(jdm);
        }
    }
}