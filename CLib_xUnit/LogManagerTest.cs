using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using System;
using System.IO;
using System.Linq;
using Xunit;

public class LogManagerTests
{
    [Fact]
    public void Test_FileLogger_CreatesLogFile()
    {
        // Arrange
        var fileSettings = new FileSettings
        {
            LogFolder = "TestLogs",
            LogFileName = "test-log-.txt",
            RetentionDays = 1,
            FileSizeLimitBytes = 1024,
            Buffered = false,
            FlushToDiskInterval = TimeSpan.FromSeconds(1)
        };

        LogManager.Instance.FileSettings = fileSettings;
        LogManager.Instance.LogType = LogType.File;
        Log.ConfigureLogger();

        // Act
        Log.Information("TestSender", 0, "This is a test information log.");
        Log.Error("TestSender", -1, "This is a test error log.");

        // Assert
        Serilog.Log.CloseAndFlush(); // 로그 파일 닫기 및 플러시
        var logFiles = Directory.GetFiles(fileSettings.LogFolder, "test-log-*.txt");
        Assert.NotEmpty(logFiles);

        // Clean up
        RetryDelete(fileSettings.LogFolder);
    }

    [Fact]
    public void Test_SQLiteLogger_CreatesDatabase()
    {
        // Arrange
        var sqliteSettings = new SQLiteSettings
        {
            SqliteDbPath = "TestLogs/test-logs.db",
            TableName = "TestLogs",
            StoreTimestampInUtc = false
        };

        LogManager.Instance.SQLiteSettings = sqliteSettings;
        LogManager.Instance.LogType = LogType.SQLite;
        Log.ConfigureLogger();

        // Act
        Log.Information("TestSender", 0, "This is a test information log.");
        Log.Error("TestSender", -1, "This is a test error log.");

        // Assert
        Serilog.Log.CloseAndFlush(); // 로그 파일 닫기 및 플러시
        Assert.True(File.Exists(sqliteSettings.SqliteDbPath));

        // Clean up
        RetryDelete(sqliteSettings.SqliteDbPath);
    }

    [Fact]
    public void Test_LogLevels()
    {
        // Arrange
        using (TestCorrelator.CreateContext())
        {
            var fileSettings = new FileSettings
            {
                LogFolder = "TestLogs",
                LogFileName = "test-log-.txt",
                RetentionDays = 1,
                FileSizeLimitBytes = 1024,
                Buffered = false,
                FlushToDiskInterval = TimeSpan.FromSeconds(1)
            };

            LogManager.Instance.FileSettings = fileSettings;
            LogManager.Instance.LogType = LogType.File;

            // TestCorrelator를 사용하기 위해 로그 설정에 TestCorrelator sink 추가
            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.TestCorrelator()
                .WriteTo.File(
                    path: Path.Combine(fileSettings.LogFolder, fileSettings.LogFileName),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: fileSettings.RetentionDays,
                    fileSizeLimitBytes: fileSettings.FileSizeLimitBytes,
                    buffered: fileSettings.Buffered,
                    flushToDiskInterval: fileSettings.FlushToDiskInterval,
                    outputTemplate: fileSettings.OutputTemplate)
                .CreateLogger();

            // Act
            Log.Verbose("TestSender", 0, "This is a verbose log.");
            Log.Information("TestSender", 0, "This is an information log.");
            Log.Warning("TestSender", 0, "This is a warning log.");
            Log.Error("TestSender", -1, "This is an error log.");
            Log.Warning("TestSender", -2, "This is an error log."); // 추가 테스트 항목

            // Assert
            var logEvents = TestCorrelator.GetLogEventsFromCurrentContext().ToList();

            Assert.Contains(logEvents, e => e.Level == LogEventLevel.Verbose
                                            && e.MessageTemplate.Text == "This is a verbose log."
                                            && e.Properties["Sender"].ToString() == "\"TestSender\""
                                            && e.Properties["ErrorCode"].ToString() == "0");
            Assert.Contains(logEvents, e => e.Level == LogEventLevel.Information
                                            && e.MessageTemplate.Text == "This is an information log."
                                            && e.Properties["Sender"].ToString() == "\"TestSender\""
                                            && e.Properties["ErrorCode"].ToString() == "0");
            Assert.Contains(logEvents, e => e.Level == LogEventLevel.Warning
                                            && e.MessageTemplate.Text == "This is a warning log."
                                            && e.Properties["Sender"].ToString() == "\"TestSender\""
                                            && e.Properties["ErrorCode"].ToString() == "0");
            Assert.Contains(logEvents, e => e.Level == LogEventLevel.Error
                                            && e.MessageTemplate.Text == "This is an error log."
                                            && e.Properties["Sender"].ToString() == "\"TestSender\""
                                            && e.Properties["ErrorCode"].ToString() == "-1");
            Assert.Contains(logEvents, e => e.Level == LogEventLevel.Error
                                            && e.MessageTemplate.Text == "This is an error log."
                                            && e.Properties["Sender"].ToString() == "\"TestSender\""
                                            && e.Properties["ErrorCode"].ToString() == "-2");

            // Clean up
            Serilog.Log.CloseAndFlush();
            RetryDelete(fileSettings.LogFolder);
        }
    }

    private void RetryDelete(string path, int retryCount = 3, int delayMilliseconds = 100)
    {
        for (int i = 0; i < retryCount; i++)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                else if (File.Exists(path))
                {
                    File.Delete(path);
                }
                return;
            }
            catch (IOException)
            {
                System.Threading.Thread.Sleep(delayMilliseconds);
            }
        }
    }
}
