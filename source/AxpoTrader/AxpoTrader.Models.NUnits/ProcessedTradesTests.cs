using AxpoTrader.Models;

namespace AxpoTrader.Tests
{
    [TestFixture]
    public class ProcessedTradesTests
    {
        [Test]
        public void ProcessedTrades_ShouldInitializeTradesCorrectly()
        {
            // Arrange
            DateTime testDate = new DateTime(2020, 1, 1);

            // Act
            var processedTrades = new ProcessedTrades(testDate);

            // Assert
            Assert.That(processedTrades.DateTime, Is.EqualTo(testDate));

            Assert.That(processedTrades.Trades.Count, Is.EqualTo(24));

            // Assert
            var firstTrade = processedTrades.Trades[0];
            Assert.That(firstTrade.Id, Is.EqualTo(1));
            Assert.That(firstTrade.Time, Is.EqualTo(TimeSpan.FromHours(23)));
            Assert.That(firstTrade.VolumeSum, Is.EqualTo(0));

            for (int i = 1; i < 24; i++)
            {
                var trade = processedTrades.Trades[i];
                Assert.That(trade.Id, Is.EqualTo(i + 1));
                Assert.That(trade.Time, Is.EqualTo(TimeSpan.FromHours(i - 1)));
                Assert.That(trade.VolumeSum, Is.EqualTo(0));
            }
        }
    }
}