using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum DevType
{
    NONE = -1,
    cEIP = 0,
    PCI_Pulse,
    RTEX,
    MLink2,
    MLink3,
    SSCNET3,
    EtherCAT,
    Soft_EtherCAT,
    DAQ_SD = 8,
    DAQ_LX,
    DAQ_DX,
    CNet,

    DAQ_CP = 12,
    DAQ_LX1,
    DAQ_LX2,

    ceCM01A,
    cEIP_MDIO,

    ADLibrary,

    NodeMasterIndex = 20,

    RTEX_Slave = RTEX + NodeMasterIndex,
    MLink2_Slave = MLink2 + NodeMasterIndex,
    MLink3_Slave = MLink3 + NodeMasterIndex,
    SSCNET3_Slave = SSCNET3 + NodeMasterIndex,
    ECAT_Pulse = EtherCAT + NodeMasterIndex,

    EMPTY,

    PCI_Pulse_ALL = 31,
    //RTEX_ALL,
    //MLink3_ALL,
    //SSCNET3_ALL,
    //EtherCAT_ALL,
    //MLink2_ALL,

    EtherCAT_CMC = 40,
    EtherCAT_Driver = 41,
    ALL = 100,
}