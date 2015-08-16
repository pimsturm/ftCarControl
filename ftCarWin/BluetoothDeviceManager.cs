using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace ftCarWin {
    public class BluetoothDeviceManager {
        public static BluetoothEndPoint LocalEndpoint { get; private set; }
        public static BluetoothClient LocalClient { get; private set; }
        public static BluetoothRadio PrimaryRadio { get; private set; }
        private static readonly Guid Guid = Guid.NewGuid();

        private static readonly List<BluetoothDeviceInfo> DeviceList;

        private static readonly List<string> CommonDevicePins = new List<string>
            {
                "0000",
                "1111",
                "1234"
            };

        static BluetoothDeviceManager()
        {
            // Define common Pin codes for Bluetooth devices

            PrimaryRadio = BluetoothRadio.PrimaryRadio;
            if (PrimaryRadio == null) {
               Debug.Write("No radio hardware or unsupported software stack");
                return;
            }
            Debug.Write("Local address: " + PrimaryRadio.LocalAddress);

            // Local bluetooth MAC address 
            var mac = PrimaryRadio.LocalAddress;
            if (mac == null) {
                Debug.Write("No local Bluetooth MAC address found");
                return;
            }
            DeviceList = new List<BluetoothDeviceInfo>();
            // mac is mac address of local bluetooth device
            //LocalEndpoint = new BluetoothEndPoint(mac, BluetoothService.SerialPort);
            LocalEndpoint = new BluetoothEndPoint(mac, Guid);             
            // client is used to manage connections
            LocalClient = new BluetoothClient(LocalEndpoint);
        }


        public static void PrintAllDevices() {
            DeviceList.AddRange(LocalClient.DiscoverDevices(65536, true, true, true, true));
        }

    }
}
