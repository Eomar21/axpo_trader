namespace AxpoTrader.Models.Settings
{
    public class TraderSettings
    {
        public string CsvPath { get; set; }
        public int IntervalMinutes { get; set; }

        public static string TraderSettingSection => "TraderSettings";
    }
}