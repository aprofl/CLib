using Serilog.Events;

public class LogItem
{
    public DateTime Timestamp { get; set; }
    public int ErrorCode { get; set; }
    public string Sender { get; set; }
    public string Message { get; set; }
    public LogEventLevel Level { get; set; }

    public LogItem(LogEventLevel level, string sender, int errorCode, string message)
    {
        Timestamp = DateTime.Now;
        Sender = sender;
        ErrorCode = errorCode;        
        Message = message;
        Level = errorCode >= 0 ? level : LogEventLevel.Error;
    }
}

public static class Log
{
    public static void ConfigureLogger() => LogManager.Instance.ConfigureLogger();

    public static void Verbose(string sender, int errorCode, string message)
        => LogManager.Instance.LogEvent(new LogItem(LogEventLevel.Verbose, sender, errorCode, message));
    
    public static void Information(string sender, int errorCode, string message)
        => LogManager.Instance.LogEvent(new LogItem(LogEventLevel.Information, sender, errorCode, message));

    public static void Warning(string sender, int errorCode, string message)
        => LogManager.Instance.LogEvent(new LogItem(LogEventLevel.Warning, sender, errorCode, message));

    public static void Error(string sender, int errorCode, string message)
        => LogManager.Instance.LogEvent(new LogItem(LogEventLevel.Error, sender, errorCode, message));
}

public static class LogManagerExtensions
{
    public static void LogEvent(this LogManager logManager, LogItem logItem)
    {
        Serilog.Log.Write(logItem.Level, "Timestamp: {Timestamp}, ErrorCode: {ErrorCode}, Sender: {Sender}, Message: {Message}", logItem.Timestamp, logItem.ErrorCode, logItem.Sender, logItem.Message);
    }
}