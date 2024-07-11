using System.Reflection;
using System.Xml.Serialization;

[Serializable]
public class Singleton<T> where T : class, new()
{
    private static readonly Lazy<T> instance = new Lazy<T>(() =>
    {
        var instance = Load() ?? new T();
        var method = typeof(T).GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic);
        method?.Invoke(instance, null);
        return instance;
    });

    public static T Instance => instance.Value;

    protected Singleton() { }

    internal static T? Load()
    {
        var path = GetPath();
        try
        {
            if (!File.Exists(path))
                return null;

            using (var sr = new StreamReader(path))
            {
                var xs = new XmlSerializer(typeof(T));
                return (T?)xs.Deserialize(sr);
            }
        }
        catch (Exception ex)
        {
            //Log.app.Error($"File Load Failed : {path}\r\n\t{ex}");
            return null;
        }
    }

    internal virtual void Save()
    {
        var path = GetPath();
        try
        {
            using (StreamWriter wr = new StreamWriter(path))
            {
                var xs = new XmlSerializer(typeof(T));
                xs.Serialize(wr, this);
            }
        }
        catch (Exception ex)
        {
            //Log.app.Error($"File Save Failed : {path}\r\n\t{ex}");
        }
    }

    internal static string GetPath()
    {
        var name = typeof(T).Name;
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "COMIZOA", "Clib", $"{name}.xml");
        return path;        
    }
}
