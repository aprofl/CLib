
namespace CLib.Device
{
    public class EtherCATFactory : IDeviceFactory
    {
        public Info CreateDevice(int deviceID, string deviceName)
        {
            return new DevInfoEtherCAT(deviceID, deviceName);
        }
    }
}
