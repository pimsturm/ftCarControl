using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CmdMessenger.Commands;

namespace CmdMessenger.Test.Commands
{
    [TestClass]
    public class ReceivedCommandTest
    {
        [TestMethod, Description("Received command must contain all arguments passed to it.")]
        public void Receive01() {
            var cmd = new SendCommand(1, 2);

            cmd.AddArguments(20, 20, 20, 20);

            string[] arguments = cmd.GetCommand().TrimEnd(';').Split(',').ToArray();

            var response = new ReceivedCommand(int.Parse(arguments[0]), arguments.Skip(1).ToArray());

            string temp = cmd.GetCommand();

            Assert.AreEqual("1,20,20,20,20;", temp);

            Assert.AreEqual(20, response.ReadInt32());
            Assert.AreEqual(20, response.ReadInt32());
            Assert.AreEqual(20, response.ReadInt32());
            Assert.AreEqual(20, response.ReadInt32());
        }
    }
}
