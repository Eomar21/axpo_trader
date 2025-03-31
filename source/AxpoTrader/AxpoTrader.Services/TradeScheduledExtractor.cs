using AxpoTrader.Models.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AxpoTrader.Services
{
    internal class TradeScheduledExtractor : BackgroundService
    {
        private readonly ILogger<TradeScheduledExtractor> m_Logger;
        private readonly ITradeManager m_TradeManager;
        private readonly ICsvWriter m_CsvWriter;
        private readonly TimeSpan retryTimeInSeconds;
        private readonly TraderSettings m_TraderSettings;

        public TradeScheduledExtractor(
            ILogger<TradeScheduledExtractor> logger,
            ITradeManager tradeManager,
            IConfiguration configuration,
            ICsvWriter csvWriter,
            IOptions<TraderSettings> options)
        {
            m_Logger = logger;
            m_TradeManager = tradeManager;
            m_CsvWriter = csvWriter;
            m_TraderSettings = options.Value;
            retryTimeInSeconds = TimeSpan.FromMinutes(m_TraderSettings.IntervalMinutes);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Process();
                }
                catch (Exception ex)
                {
                    m_Logger.LogError(ex, "Error during trade extraction.");
                }

                await Task.Delay(retryTimeInSeconds, stoppingToken);
            }
        }

        private async Task Process()
        {
            var trades = await m_TradeManager.ProcessTradesAsync(DateTime.Now);
            if (trades != null)
            {
                m_CsvWriter.Write(trades);
            }
            else
            {
                m_Logger.LogWarning("No trades found.");
            }
        }
    }
}