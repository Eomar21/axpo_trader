using AxpoTrader.Models;

namespace AxpoTrader.Services
{
    public interface ITradeManager
    {
        public Task<ProcessedTrades> ProcessTradesAsync(DateTime dateTime);
    }
}