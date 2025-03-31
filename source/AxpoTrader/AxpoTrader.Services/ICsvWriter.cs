using AxpoTrader.Models;

namespace AxpoTrader.Services
{
    public interface ICsvWriter
    {
        bool Write(ProcessedTrades processedTrades);
    }
}