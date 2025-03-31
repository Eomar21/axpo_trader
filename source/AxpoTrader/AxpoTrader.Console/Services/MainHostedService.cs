using AxpoTrader.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.Threading;
using System.Threading.Tasks;

namespace AxpoTrader.Console.Services
{
    internal class MainHostedService : IHostedService
    {
        private readonly ILogger<MainHostedService> m_Logger;
        private readonly ITradeManager m_TradeManager;

        public MainHostedService(
            ILogger<MainHostedService> logger,
            ITradeManager tradeManager)
        {
            m_Logger = logger;
            m_TradeManager = tradeManager;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            m_Logger.LogInformation("MainHostedService started");


            // TODO make user input here.
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

            //var trades = await m_TradeManager.ProcessTradesAsync(DateTime.Now);






        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            m_Logger.LogInformation("MainHostedService stopped");
            return Task.CompletedTask;
        }
    }
}
