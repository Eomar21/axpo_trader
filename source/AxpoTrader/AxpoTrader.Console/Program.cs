using AxpoTrader.Console.Services;
using AxpoTrader.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace AxpoTrader.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs\\trading.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            Log.Information("Starting up the application");

            var host = Host.CreateDefaultBuilder(args)
               .ConfigureServices((context, services) =>
               {
                   services.WithEssentialServices();
                   services.AddHostedService<MainHostedService>();
               }).ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
               })
               .Build();

            await host.RunAsync();
        }
    }
}
