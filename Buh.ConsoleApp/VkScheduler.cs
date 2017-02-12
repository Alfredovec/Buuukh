using System.Collections.Generic;
using Quartz;
using Quartz.Impl;

namespace Buh.ConsoleApp
{
    public class VkScheduler
    {
        private IScheduler _scheduler;

        public void Initialize()
        {
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler();
        }

        public void Start()
        {
            _scheduler.Start();
        }

        public void AddVkJob<T>(string login, string password, int intervalHours) where T : VkJob
        {
            IDictionary<string, object> jobParams = new Dictionary<string, object>
            {
                ["login"] = login,
                ["password"] = password
            };

            var job = JobBuilder.Create<T>()
                .WithIdentity("vkJob", "vkGroup")
                .SetJobData(new JobDataMap(jobParams))
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("vkTrigger", "vkGroup")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(intervalHours)
                    .RepeatForever())
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }
    }
}
