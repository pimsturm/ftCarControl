using System.IO.Ports;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CmdMessenger.Test.CmdCommsTest {
    [TestClass]
    public class SerialCmdClientTest {
        SerialPort serial = new SerialPort();

        [Ignore]
        [TestMethod, Description("It must be possible to open and close all serial ports one at a time.")]
        public void SerialPorts01() {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports) {

                if (port != "COM5") {
                    Trace.WriteLine("Opening port " + port);
                    OpenPort(port);
                    Assert.AreEqual(true, serial.IsOpen, port);
                }
            }
        }

        private void OpenPort(string portName) {
            serial.Close();
            serial.BaudRate = 9600;
            serial.PortName = portName;
            serial.Open();
        }
    }
}
