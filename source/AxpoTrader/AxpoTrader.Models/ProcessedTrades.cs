namespace AxpoTrader.Models
{
    public class ProcessedTrades
    {
        public List<Trade> Trades { get; set; }
        public DateTime DateTime { get; set; }

        public ProcessedTrades(DateTime dateTime)
        {
            DateTime = dateTime;
            Initialize();
        }

        private void Initialize()
        {
            Trades = [new Trade() { Id = 1, Time = TimeSpan.FromHours(23), VolumeSum = 0 }];

            for (int i = 0; i < 23; i++)
            {
                Trades.Add(new Trade() { Id = i + 2, Time = TimeSpan.FromHours(i), VolumeSum = 0 });
            }
        }
    }
}