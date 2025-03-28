using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AxpoTrader.Console.Services
{
    internal class MainHostedService : IHostedService
    {
        private readonly ILogger<MainHostedService> m_logger;

        public MainHostedService(ILogger<MainHostedService> logger)
        {
            m_logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            m_logger.LogInformation("MainHostedService started");

            string name = AnsiConsole.Ask<string>("What's your [green]name[/]?");

            // Display a friendly greeting with color
            AnsiConsole.MarkupLine($"Hello, [bold yellow]{name}[/]! Welcome to the app.");

            // A simple selection prompt
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Pick an option:")
                    .AddChoices(new[] { "Option 1", "Option 2", "Option 3" })
            );

            AnsiConsole.MarkupLine($"You selected: [underline]{choice}[/]");



            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            m_logger.LogInformation("MainHostedService stopped");
            return Task.CompletedTask;
        }
    }
}
