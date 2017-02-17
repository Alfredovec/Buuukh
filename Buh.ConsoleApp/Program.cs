using System;

namespace Buh.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Please, provide vk login and password");
                Console.ReadLine();
                return;
            }

            try
            {
                var login = args[0];
                var password = args[1];

                StartBuh(login, password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }
        
        private static void StartBuh(string vkLogin, string vkPassword)
        {
            var buhScheduler = new VkScheduler();
            buhScheduler.Initialize();
            buhScheduler.Start();
            buhScheduler.AddVkJob<BuhJob>(vkLogin, vkPassword, intervalHours: 24);
        }
    }
}
