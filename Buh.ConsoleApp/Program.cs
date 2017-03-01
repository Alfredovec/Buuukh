using System;
using Buh.Domain.Services;
using Buh.Scheduler;
using Buh.Scheduler.Jobs;
using Buh.Security;

namespace Buh.ConsoleApp
{
    class Program
    {
        private static readonly PasswordProvider PasswordProvider;
        private static readonly BuhService BuhService;

        static Program()
        {
            PasswordProvider = new PasswordProvider();
            BuhService = new BuhService();
        }

        static void Main()
        {
            try
            {
                var password = PasswordProvider.GetPassword();
                Start("rud.sergey.v@gmail.com", password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }

        public static void Start(string login, string password)
        {
            var buhScheduler = new VkScheduler();
            buhScheduler.Initialize();
            buhScheduler.Start();
            buhScheduler.AddVkJob<BuhJob>(login, password, intervalHours: 24);
        }
    }
}
