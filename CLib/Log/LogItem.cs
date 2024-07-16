using CLib;
using Serilog.Events;

public class LogItem
{
    public DateTime Timestamp { get; set; }
    public int ErrorCode { get; set; }
    public string Sender { get; set; }
    public string Message { get; set; }
    public LogEventLevel Level { get; set; }

    public Exception? Exception { get; }

    public LogItem(LogEventLevel level, string sender, int errorCode, string message, Exception? exception = null)
    {
        Timestamp = DateTime.Now;
        Sender = sender;
        ErrorCode = errorCode;
        Message = message;
        Level = errorCode >= 0 ? level : LogEventLevel.Error;
        Exception = exception;
    }

    public void Compt() => Log.Debug(Sender, 0, $"{Message} compt");
    public void Compt(int errorCode) => Log.Debug(Sender, errorCode, $"{Message} end");
    public void Compt(Exception ex) => Log.Debug(Sender, 0, $"{Message} end", ex);
}