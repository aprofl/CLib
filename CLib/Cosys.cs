using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Management;

namespace CLib
{
    public static class Cosys
    {
        private static List<IDevice> devices = new List<IDevice>();
        public static void Init()
        {
            var list = DeviceManager.Instance.ScanForDevices();
            var devInfos = DeviceInfos.Instance.Devices;
            var pInfos = PlatformInfos.Instance.List;
        }
    }
           

    public static class DeviceChecker
    {
        public static List<DeviceInfo> GetInstalledDevices(List<string> detectedDeviceIds, List<DeviceInfo> devices)
        {
            return devices.Where(device => detectedDeviceIds.Contains(device.Code)).ToList();
        }
    }

    public static class PciDeviceScanner
    {
        
    }

    public class DeviceManager : Singleton<DeviceManager>
    {        
        private List<IDevice> devices;

        public DeviceManager()
        {
            devices = new List<IDevice>();
        }

        public List<string> ScanForDevices()
        {
            List<string> deviceIds = new List<string>();

            try
            {
                string query = "SELECT * FROM Win32_PnPEntity WHERE DeviceID LIKE 'PCI%'";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                ManagementObjectCollection devices = searcher.Get();

                foreach (ManagementObject device in devices)
                {
                    // 여기서 DeviceID를 가져오는데, 필요에 따라 필터링하거나 가공할 수 있음
                    string deviceId = device["DeviceID"].ToString();
                    deviceIds.Add(deviceId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error scanning PCI devices: " + ex.Message);
            }

            return deviceIds;
        }
    }

    public interface IDevice
    {
        void LoadDevice();
    }

    public class EtherCATDevice : IDevice
    {
        public void LoadDevice()
        {
            // EtherCAT 장치 초기화 및 정보 로드
        }
    }

    public static class DeviceFactory
    {
        public static IDevice CreateDevice(string deviceId)
        {
            switch (deviceId)
            {
                case "A5025032":
                    return new EtherCATDevice();
                // 기타 다른 디바이스 타입들
                default:
                    return null;
            }
        }
    }
}