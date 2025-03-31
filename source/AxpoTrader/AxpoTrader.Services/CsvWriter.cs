using AxpoTrader.Models;
using AxpoTrader.Models.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AxpoTrader.Services
{
    internal class CsvWriter : ICsvWriter
    {
        private readonly ILogger<ICsvWriter> m_Logger;
        private readonly TraderSettings m_Settings;

        public CsvWriter(ILogger<ICsvWriter> logger, IOptions<TraderSettings> settings)
        {
            m_Logger = logger;
            m_Settings = settings.Value;
        }

        public bool Write(ProcessedTrades processedTrades)
        {
            try
            {
                // Get the local time for extraction using Europe/London time zone.
                TimeZoneInfo londonTimeZone;
                try
                {
                    // Try the IANA time zone id first (works on Linux)
                    londonTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/London");
                }
                catch
                {
                    // Fallback for Windows
                    londonTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                }
                DateTime localNow = TimeZoneInfo.ConvertTime(DateTime.UtcNow, londonTimeZone);

                string fileName = $"PowerPosition_{localNow:yyyyMMdd_HHmm}.csv";
                string fullPath = Path.Combine(m_Settings.CsvPath, fileName);

                m_Logger.LogInformation($"Writing CSV file to {fullPath}");

                string directory = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var writer = new StreamWriter(fullPath))
                {
                    writer.WriteLine("Local Time,Volume");

                    foreach (var trade in processedTrades.Trades)
                    {
                        string localTime = trade.Time.ToString(@"hh\:mm");
                        writer.WriteLine($"{localTime},{trade.VolumeSum}");
                    }
                }

                m_Logger.LogInformation("CSV file written successfully.");
                return true;
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex, "Error writing CSV file");
                return false;
            }
        }
    }
}
