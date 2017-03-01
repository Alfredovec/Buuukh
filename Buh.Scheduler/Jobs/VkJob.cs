using Buh.Integration.Vk;
using Quartz;

namespace Buh.Scheduler.Jobs
{
    public abstract class VkJob : IJob
    {
        protected readonly VkClient VkClient;

        protected VkJob()
        {
            VkClient = new VkClient();
        }

        protected abstract void Execute();

        public void Execute(IJobExecutionContext context)
        {
            var details = context.JobDetail;
            var login = (string)details.JobDataMap["login"];
            var password = (string)details.JobDataMap["password"];

            VkClient.Authorize(login, password);

            Execute();
        }
    }
}
