using Axpo;
using AxpoTrader.Models;
using Microsoft.Extensions.Logging;

namespace AxpoTrader.Services
{
    internal class TradeManager : ITradeManager
    {
        private readonly IPowerService m_PowerService;
        private readonly ILogger<ITradeManager> m_Logger;

        public TradeManager(IPowerService powerService, ILogger<ITradeManager> logger)
        {
            m_PowerService = powerService;
            m_Logger = logger;
        }

        public async Task<ProcessedTrades> ProcessTradesAsync(DateTime dateTime)
        {
            var processedData = new ProcessedTrades(dateTime);
            m_Logger.LogInformation("Looking for trades at {dateTime}", dateTime);
            var trades = await m_PowerService.GetTradesAsync(dateTime);
            if (trades.Any())
            {
                m_Logger.LogInformation("Processing trades at {dateTime}", dateTime);
                processedData.DateTime = dateTime;
                processedData.Trades.ForEach(x =>
                {
                    double volumeAggregate = 0;
                    foreach (var trade in trades)
                    {
                        volumeAggregate += trade.Periods.Where(p => p.Period == x.Id).Sum(p => p.Volume);
                    }
                    x.VolumeSum = volumeAggregate;
                    m_Logger.LogInformation("Trade at {tradeTime} is with volume sum of {tradeVolumeSum}", x.Time, x.VolumeSum.ToString());
                });
                return processedData;
            }
            m_Logger.LogWarning("Could not find any for trades at {dateTime}", dateTime);

            return processedData;
        }
    }
}