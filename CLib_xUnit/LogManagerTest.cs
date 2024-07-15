using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using Xunit.Abstractions;
using CLib;

public class LogManagerTests : ComplexityTestBase
{
    public LogManagerTests(ITestOutputHelper output) : base(output) { }
    protected override string GetCodeFilePath()
        => Path.Combine("Log", $"{nameof(LogManager)}.cs");
    
    [Fact]
    public void Test_FileLogger_CreatesLogFile()
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
        CLib.Log.ConfigureLogger();

        CLib.Log.Information("TestSender", 0, "This is a test information log.");
        CLib.Log.Error("TestSender", -1, "This is a test error log.");

        Serilog.Log.CloseAndFlush();
        var logFiles = Directory.GetFiles(fileSettings.LogFolder, "test-log-*.txt");
        Assert.NotEmpty(logFiles);
        RetryDelete(fileSettings.LogFolder);
    }

    [Fact]
    public void Test_SQLiteLogger_CreatesDatabase()
    {
        var sqliteSettings = new SQLiteSettings
        {
            SqliteDbPath = "TestLogs/test-logs.db",
            TableName = "TestLogs",
            StoreTimestampInUtc = false
        };
        LogManager.Instance.SQLiteSettings = sqliteSettings;
        LogManager.Instance.LogType = LogType.SQLite;
        CLib.Log.ConfigureLogger();

        CLib.Log.Information("TestSender", 0, "This is a test information log.");
        CLib.Log.Error("TestSender", -1, "This is a test error log.");

        Serilog.Log.CloseAndFlush();
        Assert.True(File.Exists(sqliteSettings.SqliteDbPath));
        RetryDelete(sqliteSettings.SqliteDbPath);
    }

    [Fact]
    public void Test_LogLevels()
    {
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

            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
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

            CLib.Log.Verbose("TestSender", 0, "This is a verbose log.");
            CLib.Log.Information("TestSender", 0, "This is an information log.");
            CLib.Log.Warning("TestSender", 0, "This is a warning log.");
            CLib.Log.Error("TestSender", -1, "This is an error log.");
            CLib.Log.Error("TestSender", -2, "This is an error log.");
            var exception = new InvalidOperationException("Test exception");
            CLib.Log.Error("TestSender", -3, "This is an error log with exception.", exception);

            var logEvents = TestCorrelator.GetLogEventsFromCurrentContext().ToList();
            Assert.Contains(logEvents, e => e.Level == LogEventLevel.Verbose
                                            && e.MessageTemplate.Text == "Time: {Time}, Sender: {Sender}, ErrorCode: {ErrorCode}, Message: {Message}"
                                            && e.Properties["Sender"].ToString() == "\"TestSender\""
                                            && e.Properties["ErrorCode"].ToString() == "0");
            Assert.Contains(logEvents, e => e.Level == LogEventLevel.Information
                                            && e.MessageTemplate.Text == "Time: {Time}, Sender: {Sender}, ErrorCode: {ErrorCode}, Message: {Message}"
                                            && e.Properties["Sender"].ToString() == "\"TestSender\""
                                            && e.Properties["ErrorCode"].ToString() == "0");
            Assert.Contains(logEvents, e => e.Level == LogEventLevel.Warning
                                            && e.MessageTemplate.Text == "Time: {Time}, Sender: {Sender}, ErrorCode: {ErrorCode}, Message: {Message}"
                                            && e.Properties["Sender"].ToString() == "\"TestSender\""
                                            && e.Properties["ErrorCode"].ToString() == "0");
            Assert.Contains(logEvents, e => e.Level == LogEventLevel.Error
                                            && e.MessageTemplate.Text == "Time: {Time}, Sender: {Sender}, ErrorCode: {ErrorCode}, Message: {Message}"
                                            && e.Properties["Sender"].ToString() == "\"TestSender\""
                                            && e.Properties["ErrorCode"].ToString() == "-1");
            Assert.Contains(logEvents, e => e.Level == LogEventLevel.Error
                                            && e.MessageTemplate.Text == "Time: {Time}, Sender: {Sender}, ErrorCode: {ErrorCode}, Message: {Message}"
                                            && e.Properties["Sender"].ToString() == "\"TestSender\""
                                            && e.Properties["ErrorCode"].ToString() == "-2");
            Assert.Contains(logEvents, e => e.Level == LogEventLevel.Error
                                            && e.MessageTemplate.Text == "Time: {Time}, Sender: {Sender}, ErrorCode: {ErrorCode}, Message: {Message}"
                                            && e.Properties["Sender"].ToString() == "\"TestSender\""
                                            && e.Properties["ErrorCode"].ToString() == "-3"
                                            && e.Exception == exception);
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
                    Directory.Delete(path, true);
                else if (File.Exists(path))
                    File.Delete(path);
                return;
            }
            catch (IOException)
            {
                Thread.Sleep(delayMilliseconds);
            }
        }
    }
}
