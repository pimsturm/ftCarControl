using InTheHand.Net;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ftCarWin {
    /// <summary>
    /// A BlueTooth device
    /// </summary>
    public sealed class BtDevice {
        /// <summary>
        /// Specifies whether the device is authenticated, paired, or bonded. All authenticated devices are remembered.
        /// </summary>
        public Boolean Authenticated { get; set; }

        /// <summary>
        /// The device identifier
        /// </summary>
        public BluetoothAddress DeviceAddress { get; set; }

        /// <summary>
        /// The name of the device
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Specifies whether the device is a remembered device. Not all remembered devices are authenticated. 
        /// </summary>
        public Boolean Remembered { get; set; }

        public BtDevice(BluetoothDeviceInfo device) {
            Authenticated = device.Authenticated;
            DeviceAddress = device.DeviceAddress;
            DeviceName = device.DeviceName;
            Remembered = device.Remembered;
        }

        public BtDevice(){}
    }
}
