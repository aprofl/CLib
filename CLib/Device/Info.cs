
namespace CLib.Device
{
    public abstract class Info
    {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public DevType Platform { get; set; }

        protected Info(int deviceID, string deviceName)
        {
            DeviceID = deviceID;
            DeviceName = deviceName;
        }

        public abstract bool Init();

        public abstract void DeviceLoad();
        
        public abstract void DeviceUnload();

        public Version Version { get; set; } = new Version();
    }
}