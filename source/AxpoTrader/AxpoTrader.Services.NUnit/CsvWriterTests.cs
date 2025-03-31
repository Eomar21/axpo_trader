using AxpoTrader.Models;
using AxpoTrader.Models.Settings;
using AxpoTrader.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace AxpoTrader.Tests
{
    [TestFixture]
    public class CsvWriterTests
    {
        private string _tempFolder;

        [SetUp]
        public void Setup()
        {
            _tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempFolder);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the temporary folder after the test.
            if (Directory.Exists(_tempFolder))
            {
                Directory.Delete(_tempFolder, true);
            }
        }

        [Test]
        public void Write_CreatesValidCsvFile()
        {
            TraderSettings settings = new TraderSettings
            {
                CsvPath = _tempFolder,
                IntervalMinutes = 5
            };

            IOptions<TraderSettings> options = Options.Create(settings);
            ILogger<ICsvWriter> logger = NullLogger<ICsvWriter>.Instance;
            var csvWriter = new CsvWriter(logger, options);

            ProcessedTrades processedTrades = new ProcessedTrades(DateTime.Now);

            // Set some test volume values. Here, we're just doing trade.Id * 10.
            foreach (var trade in processedTrades.Trades)
            {
                trade.VolumeSum = trade.Id * 10;
            }

            // Act: Write the CSV file.
            bool result = csvWriter.Write(processedTrades);

            // Assert: The method returns true.
            Assert.IsTrue(result, "CsvWriter.Write should return true");

            // Find the CSV file created in the temporary folder.
            var files = Directory.GetFiles(_tempFolder, "PowerPosition_*.csv");
            Assert.IsNotEmpty(files, "CSV file was not created in the expected folder");

            // Read the file's contents.
            string[] lines = File.ReadAllLines(files.First());

            // There should be 25 lines (1 header + 24 rows for trades).
            Assert.That(lines.Length, Is.EqualTo(25), "CSV file should have 25 lines (1 header + 24 trades)");

            // Check the header.
            Assert.That(lines[0], Is.EqualTo("Local Time,Volume"));

            // Check the first data row.
            // First trade (Id=1) is set to VolumeSum = 10 and Time is 23:00.
            Assert.That(lines[1], Is.EqualTo("23:00,10"), "First data row should be '23:00,10'");

            // Check the second data row.
            // Second trade (Id=2) should be at 00:00 with VolumeSum = 20.
            Assert.That(lines[2], Is.EqualTo("00:00,20"), "Second data row should be '00:00,20'");
        }
    }
}