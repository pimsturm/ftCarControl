using InTheHand.Net;
using InTheHand.Net.Sockets;
using System;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="BtDevice"/>
        /// </summary>
        /// <param name="device">Properties of the Buetooth device</param>
        public BtDevice(BluetoothDeviceInfo device) {
            Authenticated = device.Authenticated;
            DeviceAddress = device.DeviceAddress;
            DeviceName = device.DeviceName;
            Remembered = device.Remembered;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BtDevice"/>
        /// </summary>
        public BtDevice(){}
    }
}
