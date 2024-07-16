using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CLib.Device
{
    public class Manager
    {
        string name = "Device.Manager";
        private Dictionary<DevType, IDeviceFactory>? devFactory = null;        
        List<Info> _devices = new List<Info>();
        Dictionary<DevType, List<Info>> _dic = new Dictionary<DevType, List<Info>>();

        internal ReadOnlyCollection<Info>? List { get; private set; }
        internal ReadOnlyDictionary<DevType, List<Info>>? Dic { get; private set; }

        public Manager()
        {
            RegisterDeviceFactories();
        }

        private void RegisterDeviceFactories()
        {
            devFactory = new Dictionary<DevType, IDeviceFactory>();
            var devTypeValues = Enum.GetValues(typeof(DevType)).Cast<DevType>();
            var assembly = Assembly.GetExecutingAssembly();

            foreach (var devType in devTypeValues)
            {
                if (devType == DevType.NONE || devType == DevType.NodeMasterIndex)
                    continue;

                var factoryClassName = $"DevFactory_{devType}";
                var factoryType = assembly.GetType(factoryClassName);

                if (factoryType != null && typeof(IDeviceFactory).IsAssignableFrom(factoryType))
                {
                    if (Activator.CreateInstance(factoryType) is IDeviceFactory iFac)
                        devFactory[devType] = iFac;
                }
            }
        }

        internal void Init()
        {
            var log = Log.Debug(name, $"{nameof(Init)}");
            List = new ReadOnlyCollection<Info>(_devices);
            Dic = new ReadOnlyDictionary<DevType, List<Info>>(_dic);
            log.Compt();
        }


        internal void UpdateInstalledDevices()
        {
            //var log = Log.Add(nameof(GetInstalledDeviceList)).S(nameof(Cosys));
            var devs = new ManagementObjectSearcher(@"root\CIMV2", $"SELECT * FROM Win32_PnPEntity WHERE Manufacturer LIKE '{"COMIZOA"}%'").Get();
            var drivers = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_PnPSignedDriver WHERE Manufacturer LIKE '{"COMIZOA"}%'").Get();

            string devID = string.Empty;
            string devCode = string.Empty;

            try
            {
                foreach (var dev in devs)
                {
                    devID = dev["DeviceID"].ToString();
                    devCode = devID.Substring(devID.IndexOf("SUBSYS_") + 7, 8);
                    var tempDevType = Infos.Devices.Instance.List?.Find(x => x.Code.Equals(devCode, StringComparison.OrdinalIgnoreCase));
                    if (tempDevType != null)
                    {
                        var platform = Infos.Platforms.Instance.dicPlatform[tempDevType.DevType];
                        platform.IsInstalled = true;
                        foreach (var drv in drivers)
                        {
                            devID = drv["DeviceID"].ToString();
                            devCode = devID.Substring(devID.IndexOf("SUBSYS_") + 7, 8);
                            if (devCode.Equals(tempDevType.Code, StringComparison.OrdinalIgnoreCase))
                            {
                                platform.IsDriverInstalled = true;
                                platform.DriverVersion = new Version(drv["DriverVersion"].ToString());
                                break;
                            }
                        }
                    }
                }
                TimeLog.Add("Find Devs end");
                GetDaemon_cEIP();
                GetDaemon_SoftECat();
                GetDaemon_StandAloneEtherCAT();
                TimeLog.Add("GetDaemon end");
            }
            catch (Exception ex)
            {
                //log.Compt(ex);
            }
            finally
            {
                drivers = null;
                devs = null;
                //log.Compt(0);
            }
        }


        private static bool GetDaemon_cEIP()
        {
            Process[] ps = Process.GetProcessesByName("ceSDKDaemon");
            if (ps.Length > 0)
                Infos.Platforms.Instance.dicPlatform[DevType.cEIP].IsInstalled = true;
            return ps.Length > 0;
        }


        private static bool GetDaemon_SoftECat()
        {
            Process[] ps = Process.GetProcessesByName("ComiSWEcatDaemon");
            if (ps.Length > 0)
                Infos.Platforms.Instance.dicPlatform[DevType.Soft_EtherCAT].IsInstalled = true;
            return ps.Length > 0;
        }


        private static void GetDaemon_StandAloneEtherCAT()
        {
            Process[] ps = Process.GetProcessesByName("ComiSAEcatDaemon");
            if (ps.Length > 0)
                Infos.Platforms.Instance.dicPlatform[DevType.EtherCAT].IsInstalled = true;
        }
    }
}