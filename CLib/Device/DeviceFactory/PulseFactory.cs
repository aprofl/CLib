using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLib.Device
{
    public class PulseFactory : IDeviceFactory
    {
        public Info CreateDevice(int deviceID, string deviceName)
        {
            return new DevInfoPulse(deviceID, deviceName);
        }
    }
}
