using System.Reflection;
using System.Xml.Serialization;

namespace CLib.Infos
{
    /// <summary>
    /// Singleton Base Class
    /// </summary>
    /// <remarks>
    /// <para>%AppData%\COMIZOA\CLib\l 파일의 정보 참조</para>
    /// </remarks>
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
            if (!File.Exists(path))
                return null;

            try
            {
                return DeserializeFromFile(path);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, $"{typeof(T).Name}: Load Failed : {path}");
                return null;
            }
        }

        private static T? DeserializeFromFile(string path)
        {
            using (var sr = new StreamReader(path))
            {
                var xs = new XmlSerializer(typeof(T));
                return (T?)xs.Deserialize(sr);
            }
        }

        internal virtual void Save()
        {
            var path = GetPath();
            SaveToFile(path);
        }

        private void SaveToFile(string path)
        {
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
                Serilog.Log.Error(ex, $"{typeof(T).Name}: Save Failed : {path}");
            }
        }

        internal static string GetPath()
        {
            var name = typeof(T).Name;
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "COMIZOA", "Clib", $"{name}.xml");
        }
    }
}