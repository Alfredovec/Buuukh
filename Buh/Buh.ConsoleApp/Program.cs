using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;

namespace Buh.ConsoleApp
{
    class Program
    {
        private static IScheduler _scheduler;
        private static Random _random;

        private static string _email;
        private static string _password;

        static void Main(string[] args)
        {
            _email = args[0];
            _password = args[1];
            
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler();
            _scheduler.Start();
            Console.WriteLine("Starting Scheduler");
            _random = new Random();

            AddJob();
        }


        public static void AddJob()
        {
            var job = JobBuilder.Create<PostBuhJob>()
                .WithIdentity("myJob", "group1")
                .Build();
            
            var trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(24)
                    .RepeatForever())
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        internal class PostBuhJob : IMyJob
        {
            public void Execute(IJobExecutionContext context)
            {
                Send();
            }

            private void Send()
            {
                var randomHours = _random.Next(24);
                var randomMinutes = _random.Next(60);
                var publishDate = DateTime.Now.AddHours(randomHours).AddMinutes(randomMinutes);
                var appID = (ulong)5749376;
                var scope = Settings.All;

                var vk = new VkApi();
                vk.Authorize(new ApiAuthParams
                {
                    ApplicationId = appID,
                    Login = _email,
                    Password = _password,
                    Settings = scope
                });

                vk.Wall.Post(
                    new WallPostParams
                    {
                        OwnerId = -134042408,
                        Message = "Б" + string.Join("", Enumerable.Range(0, _random.Next(2, 6)).Select(x => "y")) + "х!",
                        FromGroup = true,
                        PublishDate = publishDate
                    });
            }
        }

        internal interface IMyJob : IJob
        {
        }
    }
}
