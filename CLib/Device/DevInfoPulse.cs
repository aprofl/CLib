using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLib.Device
{
    public class DevInfoPulse : Info
    {
        public DevInfoPulse(int deviceID, string deviceName) : base(deviceID, deviceName)
        {
            Platform = DevType.EtherCAT;
        }

        public override bool Init()
        {
            return true;
        }

        public override void DeviceLoad()
        {

        }
        public override void DeviceUnload()
        {

        }
    }
}
