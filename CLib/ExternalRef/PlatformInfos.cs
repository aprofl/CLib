using System.Collections.Generic;
using System.Xml.Serialization;

namespace CLib
{
    [XmlRoot("PlatformInfos")]
    public class PlatformInfos : Singleton<PlatformInfos>
    {
        public PlatformInfos()
        {
        }

        internal void Init()
        {
            if (List == null || List.Count == 0)
            {
                List = new List<Platform>()
            {
                new Platform() { Name = "cEIP", DLL = "ceSDKDLL.dll" },
                new Platform() { Name = "PCI_Pulse", Driver = "ComiLX", DLL = "cmmsdk.dll" },
                new Platform() { Name = "RTEX", Driver ="ComiRTEX,ComiNEMODrv", DLL = "ComiRTEX.dll" },
                new Platform() { Name = "MLink2", Driver ="ComiMLink2,ComiNEMODrv", DLL = "ComiMLink2.dll" },
                new Platform() { Name = "MLink3", Driver ="ComiMLink3,ComiNEMODrv,ComiM3", DLL = "ComiMLink3.dll" },
                new Platform() { Name = "SSCNET3", Driver ="ComiSSCNET3,ComiNEMODrv", DLL = "ComiSSCNET3.dll" },
                new Platform() { Name = "EtherCAT", Driver ="ComiECAT", DLL = "ComiEcatSdk.dll" },
                new Platform() { Name = "Soft_EtherCAT", DLL = "ComiSWEcatSdk.dll" },
                new Platform() { Name = "DAQ_SD", Driver ="Comidas", DLL = "ComiDll.dll" },
                new Platform() { Name = "DAQ_LX", Driver ="ComiLX", DLL = "ComiDasLx.dll" },
                new Platform() { Name = "DAQ_DX", Driver ="ComiDX", DLL = "ComiDXDll.dll" },
                new Platform() { Name = "CNet", Driver ="ComiNet", DLL = "CNETSDK.dll" },
            };
                Save();
            }

            dicPlatform.Clear();
            List?.ForEach(x => dicPlatform.Add(x.Type, x));
        }

        [XmlElement]
        public Info? Info { get; set; } = new Info();

        [XmlElement("Platform")]
        public List<Platform>? List { get; set; }
        [XmlIgnore]
        public Dictionary<DevType, Platform> dicPlatform = new Dictionary<DevType, Platform>();
    }

    public class Platform
    {
        [XmlIgnore]
        private string? name;

        [XmlAttribute]
        public string? Name
        {
            get { return name; }
            set
            {
                name = value;
                Enum.TryParse(name, out Type);
            }
        }

        [XmlIgnore]
        public DevType Type;

        [XmlAttribute]
        public string? DLL { get; set; }

        [XmlElement("Driver")]
        public List<string> Drivers { get; set; } = new List<string>();

        [XmlIgnore]
        public string Driver
        {
            get => string.Join(",", Drivers);
            set => Drivers = value.Split(",").ToList();
        }

        [XmlIgnore]
        public bool IsInstalled { get; set; }

        [XmlIgnore]
        public bool IsDriverInstalled { get; set; }
    }
}