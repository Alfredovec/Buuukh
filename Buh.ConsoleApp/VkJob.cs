using Quartz;
using VkNet;
using VkNet.Enums.Filters;

namespace Buh.ConsoleApp
{
    public abstract class VkJob : IJob
    {
        protected const ulong ApplicationId = 5749376;
        
        protected readonly VkApi Vk;

        protected VkJob()
        {
            Vk = new VkApi();
            Vk.OnTokenExpires += api => api.RefreshToken();
        }

        protected abstract void Execute();

        public void Execute(IJobExecutionContext context)
        {
            Authorize(context);
            Execute();
        }

        private void Authorize(IJobExecutionContext context)
        {
            var details = context.JobDetail;
            var login = details.JobDataMap["login"];
            var password = details.JobDataMap["password"];

            if (Vk.IsAuthorized == false)
            {
                Vk.Authorize(new ApiAuthParams
                {
                    ApplicationId = ApplicationId,
                    Login = (string)login,
                    Password = (string)password,
                    Settings = Settings.All
                });
            }
        }
    }
}
