using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CmdMessenger.Test.CmdCommsTest
{
    public class MockClient : CmdComms.CmdComms
    {
        private readonly MemoryStream stream;

        #region Constructor

        public MockClient() : base(new MockLogger()) {
            stream = new MemoryStream();
        }

        #endregion

        #region Properties

        public Stream Stream {
            get { return stream; }
        }

        #endregion

        #region Methods

        public string StreamString() {
            return Encoding.ASCII.GetString(stream.ToArray());
        }

        protected override Stream GetStream() {
            return stream;
        }

        public override Task<bool> OpenAsync() {
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(true);
            return tcs.Task;
        }

        public override bool IsOpen() {
            return true;
        }

        /// <summary>
        /// Disconnect from the device.
        /// </summary>
        public override void Close() {
            stream.Close();
        }

        /// <summary>
        /// Get a description of the communications.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// A description of the communications interface.
        /// </returns>
        protected string GetDescription() {
            return "Mock";
        }

        #endregion
    }
}
