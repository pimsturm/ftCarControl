using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CmdMessenger.Commands;
using CmdMessenger.CmdComms;
using CmdMessenger.Test.CmdCommsTest;

namespace CmdMessenger.Test
{
    public class MockCmdServer
    {
        private readonly CmdMessenger cmdMessenger;

        public MockCmdServer() {
            //  this.cmdMessenger = new CmdMessenger(new TcpCmdServer(5000));
            this.cmdMessenger.Register(
                5,
                c => this.cmdMessenger.Send(new SendCommand(6)));
        }

        public void Start() {
            this.cmdMessenger.Start();
        }

        public void Stop() {
            this.cmdMessenger.Stop();
        }
    }

    public class MockLogger : ILogger
    {
        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="message">The message</param>
        public void LogMessage(string message) { }

    }

    [TestClass]
    public class CmdMessengerTest
    {

        [Ignore]
        [TestMethod]
        public void IntegrationTest() {
            var cmdServer = new MockCmdServer();
            Task.Factory.StartNew(cmdServer.Start);

            var cmdClient = new CmdMessenger(new TcpCmdClient("127.0.0.1", 5000), new MockLogger());
            cmdClient.Start();
            IReceivedCommand command = cmdClient.Send(new SendCommand(5, 6));

            cmdClient.Stop();
            cmdServer.Stop();
        }

        [TestMethod]
        public void DataSent_Send_SendsData() {
            var client = new MockClient();
            var m = new CmdMessenger(client, new MockLogger());
            m.Send(new SendCommand(5));
            Assert.AreEqual("5;", client.StreamString());
        }

        [Ignore]
        [TestMethod]
        public void ResponseHandled_Send_ReturnsResponse() {
            var client = new MockClient();
            var m = new CmdMessenger(client, new MockLogger());
            m.Start();

            bool responseReceived = false;
            Task.Factory.StartNew(
                () =>
                {
                    var result = m.Send(new SendCommand(5, 6));
                    Assert.AreEqual(6, result.CommandId);
                    responseReceived = true;
                    m.Stop();
                });

            m.Send(new SendCommand(6));
            client.Stream.Position = client.Stream.Position - 2;

            Assert.IsTrue(TestHelper.Wait(5).Untill(() => responseReceived), "This timed out");
        }

        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(TaskCanceledException))]
        public void ResponseHandled_Send_ThrowsCmdTimeoutEx() {
            var client = new MockClient();

            var m = new CmdMessenger(client, new MockLogger());
            var result = m.Send(new SendCommand(5, 6));

            TestHelper.Wait(5);
        }

        [TestMethod]
        public void HandleUnidirectionalCommand_Listen_CommandReceived() {
            var client = new MockClient();
            var m = new CmdMessenger(client, new MockLogger());

            bool commandReceived = false;
            m.Register(6, r => { commandReceived = true; });

            m.Send(new SendCommand(6));
            client.Stream.Position = 0;
            m.Start();

            Assert.IsTrue(TestHelper.Wait(5).Untill(() => commandReceived));

            m.Stop();
        }

        [Ignore]
        [TestMethod]
        public void Test() {
            var sendMessage = new SendCommand(0, 1);
            sendMessage.AddArguments(true);          // Add a Boolean argument.
            sendMessage.AddArguments("Hello World"); // Add a String argument. 
            sendMessage.AddArguments(5);             // Add am integer argument. 
            Assert.AreEqual("0,1,Hello World,5;", sendMessage.GetCommand());

        }
    }
}
