namespace AxpoTrader.Models.Settings
{
    public class TraderSettings
    {
        public required string CsvPath { get; set; }
        public required int IntervalMinutes { get; set; }

        public static string TraderSettingSection => "TraderSettings";
    }
}
