﻿using AxpoTrader.Models.Settings;
using AxpoTrader.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AxpoTrader.Console
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

            Log.Information("Starting up the application");

            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var host = Host.CreateDefaultBuilder(args)
               .ConfigureServices((context, services) =>
               {
                   services.WithEssentialServices();
                   services.AddOptions();
                   services.Configure<TraderSettings>(config.GetSection(TraderSettings.TraderSettingSection));
               })
               .Build();

            await host.RunAsync();
        }
    }
}