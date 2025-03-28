using Axpo;
using Microsoft.Extensions.DependencyInjection;

namespace AxpoTrader.Services
{
    public static class RegistrationsExtension
    {
        public static void WithEssentialServices(this IServiceCollection services)
        {
            services.AddSingleton<IPowerService, PowerService>();
        }
    }
}
