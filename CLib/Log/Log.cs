using Serilog.Events;

public static class Log
{
    public static void ConfigureLogger() => LogManager.Instance.ConfigureLogger();

    public static void Verbose(string sender, int errorCode, string message, Exception? ex = null)
        => LogManager.Instance.Write(new LogItem(LogEventLevel.Verbose, sender, errorCode, message, ex));

    public static void Information(string sender, int errorCode, string message, Exception? ex = null)
        => LogManager.Instance.Write(new LogItem(LogEventLevel.Information, sender, errorCode, message, ex));

    public static void Warning(string sender, int errorCode, string message, Exception? ex = null)
        => LogManager.Instance.Write(new LogItem(LogEventLevel.Warning, sender, errorCode, message, ex));

    public static void Error(string sender, int errorCode, string message, Exception? ex = null)
        => LogManager.Instance.Write(new LogItem(LogEventLevel.Error, sender, errorCode, message, ex));
}

public static class LogManagerExtensions
{
    public static void Write(this LogManager logManager, LogItem logItem)
    {
        Serilog.Log.Write(logItem.Level, logItem.Exception, "Time: {Time}, Sender: {Sender}, ErrorCode: {ErrorCode}, Message: {Message}", logItem.Timestamp, logItem.Sender, logItem.ErrorCode, logItem.Message);
    }
}