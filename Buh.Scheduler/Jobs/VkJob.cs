namespace Buh.Scheduler.Jobs
{
    public abstract class VkJob : IJob
    {
        protected readonly VkService VkService;

        protected VkJob()
        {
            VkService = new VkService();
        }

        protected abstract void Execute();

        public void Execute(IJobExecutionContext context)
        {
            var details = context.JobDetail;
            var login = (string)details.JobDataMap["login"];
            var password = (string)details.JobDataMap["password"];

            VkService.Authorize(login, password);

            Execute();
        }
    }
}
