using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// Device의 기본 정보 제공
/// </summary>
/// <remarks>
/// <para>%AppData%\COMIZOA\CLib\DeviceInfos.xml 파일의 정보 참조</para>
/// <para>EtherCAT 모듈의 경우, Configuration 과정에서 해당 파일에 슬레이브 정보 추가</para>
/// </remarks>
[Serializable]
[XmlRoot("DeviceList")]
public class DeviceInfos : Singleton<DeviceInfos>
{    
    void Init()
    {
        if (Devices != null && Devices.Count > 0)
            return;

        Devices = new List<DeviceInfo>();
        Devices?.Add(new DeviceInfo("A5025032", "COMI-LX502", "PCI_Pulse"));
        Devices?.Add(new DeviceInfo("A5045032", "COMI-LX504", "PCI_Pulse"));
        Devices?.Add(new DeviceInfo("A5085032", "COMI-LX508", "PCI_Pulse"));
        Devices?.Add(new DeviceInfo("A5345032", "COMI-LX534", "PCI_Pulse"));
        Devices?.Add(new DeviceInfo("A5445032", "COMI-LX504a", "PCI_Pulse"));
        Devices?.Add(new DeviceInfo("E5045032", "COMI-GX504", "PCI_Pulse"));
        Devices?.Add(new DeviceInfo("E5085032", "COMI-GX508", "PCI_Pulse"));
        Devices?.Add(new DeviceInfo("FE505032", "COMI-SRC508", "PCI_Pulse"));
        Devices?.Add(new DeviceInfo("A5305031", "COMI-LX530S", "MLink2"));
        Devices?.Add(new DeviceInfo("A5305032", "COMI-LX530", "MLink3"));
        Devices?.Add(new DeviceInfo("A5405032", "COMI-LX540", "SSCNET3"));
        Devices?.Add(new DeviceInfo("A5215032", "COMI-LX521", "RTEX"));
        Devices?.Add(new DeviceInfo("A5315031", "COMI-LX531S", "MLink2"));
        Devices?.Add(new DeviceInfo("A5315032", "COMI-LX531S", "MLink3"));
        Devices?.Add(new DeviceInfo("A5415032", "COMI-LX541", "SSCNET3"));
        Devices?.Add(new DeviceInfo("A5205032", "COMI-LX520", "RTEX"));
        Devices?.Add(new DeviceInfo("A55F5032", "COMI-LX550", "EtherCAT"));
        Devices?.Add(new DeviceInfo("A5505032", "COMI-LX551", "EtherCAT"));
        Devices?.Add(new DeviceInfo("A5515032", "COMI-LX552", "EtherCAT"));
        Devices?.Add(new DeviceInfo("A5525032", "COMI-LX554", "EtherCAT"));
        Devices?.Add(new DeviceInfo("A5565032", "COMI-LX552(40)", "EtherCAT"));
        Devices?.Add(new DeviceInfo("A5535032", "COMI-LX554(48)", "EtherCAT"));
        Devices?.Add(new DeviceInfo("A4125032", "COMI-LX412a", "CNet"));
        Devices?.Add(new DeviceInfo("A4135032", "COMI-LX412b", "CNet"));
        Devices?.Add(new DeviceInfo("B4025032", "COMI-SD402", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B4035032", "COMI-SD403", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B4045032", "COMI-SD404", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B4145032", "COMI-SD414", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B4245032", "COMI-SD424", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B4345032", "COMI-SD434", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("C4015032", "COMI-CP401", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("C1015032", "COMI-CP101", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("C2015032", "COMI-CP201", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("C3015032", "COMI-CP301", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("C3025032", "COMI-CP302", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("C5015032", "COMI-CP501", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B1015032", "COMI-SD101", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B1025032", "COMI-SD102", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B1035032", "COMI-SD103", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B1045032", "COMI-SD104", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B2015032", "COMI-SD201", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B2025032", "COMI-SD202", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B2035032", "COMI-SD203", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B2095032", "COMI-SD209", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B3015032", "COMI-SD301", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B5015032", "COMI-SD501", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("B5025032", "COMI-SD502", "DAQ_SD"));
        Devices?.Add(new DeviceInfo("A1015032", "COMI-LX101", "DAQ_LX"));
        Devices?.Add(new DeviceInfo("A1025032", "COMI-LX102", "DAQ_LX"));
        Devices?.Add(new DeviceInfo("A1035032", "COMI-LX103", "DAQ_LX"));
        Devices?.Add(new DeviceInfo("A2015032", "COMI-LX201", "DAQ_LX"));
        Devices?.Add(new DeviceInfo("A2025032", "COMI-LX202", "DAQ_LX"));
        Devices?.Add(new DeviceInfo("A2035032", "COMI-LX203", "DAQ_LX"));
        Devices?.Add(new DeviceInfo("A3015032", "COMI-LX301", "DAQ_LX"));
        Devices?.Add(new DeviceInfo("A4015032", "COMI-LX401", "DAQ_LX"));
        Devices?.Add(new DeviceInfo("A4025032", "COMI-LX402", "DAQ_LX"));
        Devices?.Add(new DeviceInfo("A5015032", "COMI-LX501", "DAQ_LX"));
        Devices?.Add(new DeviceInfo("F1015032", "COMI-DX101", "DAQ_DX"));
        Devices?.Add(new DeviceInfo("F2015032", "COMI-DX201", "DAQ_DX"));
        Devices?.Add(new DeviceInfo("F3015032", "COMI-DX301", "DAQ_DX"));
        Devices?.Add(new DeviceInfo("F5015032", "COMI-DX501", "DAQ_DX"));

        Save();
    }

    [XmlElement]
    public Info Info { get; set; } = new Info();

    [XmlElement("DeviceInfo")]
    public List<DeviceInfo>? Devices { get; set; }
}


[Serializable]
public class DeviceInfo
{
    public DeviceInfo() { }
    public DeviceInfo(string code, string name, string platform)
    {
        Code = code;
        Name = name;
        Platform = platform;
    }

    [XmlAttribute]
    public string Code { get; set; } = string.Empty;

    [XmlAttribute]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute]
    public string Platform { get; set; } = string.Empty;

    private int di;
    [XmlAttribute("DI")]
    public int DI
    {
        get => di;
        set => di = value;
    }

    public bool ShouldSerializeDI() => di > 0;

    private int doCh;
    [XmlAttribute("DO")]
    public int DO
    {
        get => doCh; 
        set => doCh = value; 
    }

    public bool ShouldSerializeDO() => doCh > 0;

    private int ai;
    [XmlAttribute("AI")]
    public int AI
    {
        get => ai; 
        set => ai = value; 
    }
    
    public bool ShouldSerializeAI() => ai > 0;

    private int ao;
    [XmlAttribute("AO")]
    public int AO
    {
        get => ao; 
        set => ao = value;
    }

    public bool ShouldSerializeAO() => ao > 0;

    private int counter;
    [XmlAttribute("Counter")]
    public int Counter
    {
        get => counter;
        set => counter = value;
    }

    public bool ShouldSerializeCounter() => counter > 0;

    private int serial;
    [XmlAttribute("Serial")]
    public int Serial
    {
        get => serial; 
        set => serial = value;
    }

    public bool ShouldSerializeSerial() => serial > 0;

    private int axis;
    [XmlAttribute("Axis")]
    public int Axis
    {
        get => axis;
        set => axis = value;
    }

    public bool ShouldSerializeAxis() => axis > 0;
}

