using CLib.Infos;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.IO;
using System.Xml.Serialization;

namespace CLib
{
    public class LogManager : Singleton<LogManager>
    {
        private void Init()
        {
            ConfigureLogger();
        }

        public void ConfigureLogger()
        {
            ILogFactory logFactory = LogType switch
            {
                LogType.SQLite => new SQLiteLogFactory(SQLiteSettings),
                LogType.File => new FileLogFactory(FileSettings),
                _ => throw new ArgumentException("Invalid log type specified."),
            };

            Serilog.Log.Logger = logFactory.CreateLoggerConfiguration().CreateLogger();
            Save();
        }

        LogType _logType = LogType.File;
        [XmlElement("LogType")]
        public LogType LogType
        {
            get => _logType;
            set
            {
                _logType = value;
                Serilog.Log.CloseAndFlush();
                ConfigureLogger();
            }
        }

        [XmlElement("FileSettings")]
        public FileSettings FileSettings { get; set; } = new FileSettings();

        [XmlElement("SQLiteSettings")]
        public SQLiteSettings SQLiteSettings { get; set; } = new SQLiteSettings();
    }

    public interface ILogFactory
    {
        LoggerConfiguration CreateLoggerConfiguration();
    }

    public class FileLogFactory : ILogFactory
    {
        private readonly FileSettings fileSettings;

        public FileLogFactory(FileSettings fileSettings)
        {
            this.fileSettings = fileSettings;
        }

        public LoggerConfiguration CreateLoggerConfiguration()
        {
            if (!Directory.Exists(fileSettings.LogFolder))
                Directory.CreateDirectory(fileSettings.LogFolder);

            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                //.Enrich.With(new CustomLogEnricher())
                .WriteTo.File(
                    path: Path.Combine(fileSettings.LogFolder, fileSettings.LogFileName),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: fileSettings.RetentionDays,
                    fileSizeLimitBytes: fileSettings.FileSizeLimitBytes,
                    buffered: fileSettings.Buffered,
                    flushToDiskInterval: fileSettings.FlushToDiskInterval,
                    outputTemplate: fileSettings.OutputTemplate);
        }
    }

    public class SQLiteLogFactory : ILogFactory
    {
        private readonly SQLiteSettings sqliteSettings;

        public SQLiteLogFactory(SQLiteSettings sqliteSettings)
        {
            this.sqliteSettings = sqliteSettings;
        }

        public LoggerConfiguration CreateLoggerConfiguration()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                //.Enrich.With(new CustomLogEnricher())
                .WriteTo.SQLite(
                    sqliteDbPath: sqliteSettings.SqliteDbPath,
                    tableName: sqliteSettings.TableName,
                    storeTimestampInUtc: sqliteSettings.StoreTimestampInUtc);
        }
    }

    //public class CustomLogEnricher : ILogEventEnricher
    //{
    //    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    //    {
    //        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Time", DateTime.Now));
    //    }
    //}

    public class FileSettings
    {
        [XmlElement("LogFolder")]
        public string LogFolder { get; set; } = "Logs";

        [XmlElement("LogFileName")]
        public string LogFileName { get; set; } = "log-.txt";

        [XmlElement("RetentionDays")]
        public int RetentionDays { get; set; } = 7;

        [XmlElement("FileSizeLimitBytes")]
        public long? FileSizeLimitBytes { get; set; } = 1024000;

        [XmlElement("Buffered")]
        public bool Buffered { get; set; } = true;

        [XmlElement("FlushToDiskInterval")]
        public TimeSpan FlushToDiskInterval { get; set; } = TimeSpan.FromSeconds(60);

        [XmlElement("OutputTemplate")]
        public string OutputTemplate { get; set; } = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";
    }

    public class SQLiteSettings
    {
        [XmlElement("SqliteDbPath")]
        public string SqliteDbPath { get; set; } = "logs.db;";

        [XmlElement("TableName")]
        public string TableName { get; set; } = "Logs";

        [XmlElement("StoreTimestampInUtc")]
        public bool StoreTimestampInUtc { get; set; } = true;
    }
}