using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLib.Device
{
    internal interface IDeviceFactory
    {
        Info CreateDevice(int deviceID, string deviceName);
    }
}
