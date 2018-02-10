using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CmdMessenger.Test
{
    [TestClass]
    public class EscapingTest
    {
        [TestMethod]
        public void UnescapeParameters() {
            var escaping = new Escaping(',', ';', '\\');

            string[] result = escaping.GetUnescapedParameters(Encoding.ASCII.GetBytes("10,He\\,llo"));

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("10", result[0], "The first parameter should be 10.");
            Assert.AreEqual("He,llo", result[1], @"The second parameter should be ""He,llo"" ");
        }

        [TestMethod]
        public void CanEscapeParameters() {
            var escaping = new Escaping(',', ';', '/');

            string result = escaping.EscapeParameters("10", "He,llo");

            Assert.AreEqual("10,He/,llo;", result);
        }

        [TestMethod]
        public void GetCommands() {
            var escaping = Escaping.Default;

            IEnumerable<string> commands = escaping.GetCommands("10101010101;101010101");

            Assert.AreEqual(1, commands.Count());
        }

        [TestMethod]
        public void GetCommandsStream() {
            var escaping = Escaping.Default;
            var queue = new Queue<byte>();
            Encoding.ASCII.GetBytes("10101010101;101010101;").ToList().ForEach(queue.Enqueue);

            // byte[] commands1 = escaping.GetCommand(queu).First();
            // byte[] commands2 = escaping.GetCommand(queu).First();
        }
    }
}
