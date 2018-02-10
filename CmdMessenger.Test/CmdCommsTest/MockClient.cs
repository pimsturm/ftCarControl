using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CmdMessenger.Test.CmdCommsTest
{
    public class MockClient : CmdComms.CmdComms
    {
        private readonly MemoryStream stream;

        #region Constructor

        public MockClient() {
            this.stream = new MemoryStream();
        }

        #endregion

        #region Properties

        public Stream Stream {
            get { return this.stream; }
        }

        #endregion

        #region Methods

        public string StreamString() {
            return Encoding.ASCII.GetString(this.stream.ToArray());
        }

        protected override Stream GetStream() {
            return this.stream;
        }

        public override Task OpenAsync() {
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(true);
            return tcs.Task;
        }

        /// <summary>
        /// Disconnect from the device.
        /// </summary>
        public override void Close() {
            this.stream.Close();
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
