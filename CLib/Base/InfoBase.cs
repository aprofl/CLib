using System.Xml.Serialization;

[Serializable]
public class Info
{
    [XmlAttribute("Version")]
    public string VersionBase
    {
        get => Version.ToString();
        set => Version = new Version(value);
    }

    [XmlIgnore]
    internal Version Version { get; set; } = new Version("1.0.0.0");
}