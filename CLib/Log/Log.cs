using Serilog.Events;

namespace CLib
{
    public static class Log
    {
        public static void ConfigureLogger() => LogManager.Instance.ConfigureLogger();

        public static LogItem Verbose(string sender, int errorCode, string message, Exception? ex = null)
            => LogManager.Instance.Write(new LogItem(LogEventLevel.Verbose, sender, errorCode, message, ex));

        public static LogItem Debug(string sender, string message, Exception? ex = null)
            => LogManager.Instance.Write(new LogItem(LogEventLevel.Debug, sender, 0, message, ex));

        public static LogItem Debug(string sender, int errorCode, string message, Exception? ex = null)
            => LogManager.Instance.Write(new LogItem(LogEventLevel.Debug, sender, errorCode, message, ex));

        public static LogItem Information(string sender, int errorCode, string message, Exception? ex = null)
            => LogManager.Instance.Write(new LogItem(LogEventLevel.Information, sender, errorCode, message, ex));

        public static LogItem Warning(string sender, int errorCode, string message, Exception? ex = null)
            => LogManager.Instance.Write(new LogItem(LogEventLevel.Warning, sender, errorCode, message, ex));

        public static LogItem Error(string sender, int errorCode, string message, Exception? ex = null)
            => LogManager.Instance.Write(new LogItem(LogEventLevel.Error, sender, errorCode, message, ex));
    }

    public static class LogManagerExtensions
    {
        public static LogItem Write(this LogManager logManager, LogItem logItem)
        {
            Serilog.Log.Write(logItem.Level, logItem.Exception, "Time: {Time}, Sender: {Sender}, ErrorCode: {ErrorCode}, Message: {Message}", logItem.Timestamp, logItem.Sender, logItem.ErrorCode, logItem.Message);
            return logItem;
        }
    }
}