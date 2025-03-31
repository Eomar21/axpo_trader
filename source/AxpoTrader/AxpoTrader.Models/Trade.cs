namespace AxpoTrader.Models
{
    public class Trade
    {
        public int Id { get; set; }
        public TimeSpan Time { get; set; }
        public double VolumeSum { get; set; }
    }
}
