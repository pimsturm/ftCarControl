using System.Threading;
using System.Threading.Tasks;
using CmdMessenger.Commands;

namespace CmdMessenger.CmdComms
{

    /// <summary>
    /// An interface for interface types.
    /// </summary>
    public interface ICmdComms
    {
        /// <summary>
        /// Gets the transport channel.
        /// </summary>
        TransportChannel TransportChannel { get; }

        /// <summary>
        /// Connect to the device.
        /// </summary>
        /// <returns><code>true</code> if the connection is open.</returns>
        Task<bool> OpenAsync();

        /// <summary>
        /// Disconnect from the device.
        /// </summary>
        void Close();

        /// <summary>
        /// Writes a parameter to the serial port.
        /// </summary>
        /// <param name="command">The command to send.</param>
        /// <param name="token">Token for monitoring the cancellation status</param>
        void Send(ISendCommand command, CancellationToken token);

        /// <summary>
        /// Asynchronously, writes a command to the stream.
        /// </summary>
        /// <param name="command">The command to send.</param>
        /// <param name="token">Token for monitoring the cancellation status</param>
        /// <returns>A task indicating when the send has completed.</returns>
        Task<bool> SendAsync(ISendCommand command, CancellationToken token);

        /// <summary>
        /// Reads a command from the stream.
        /// </summary>
        /// <returns></returns>
        IReceivedCommand Read(CancellationToken token);

        /// <summary>
        /// Asynchronously, reads a command from the stream.  
        /// </summary>
        /// <returns>The command read from the stream.</returns>
        Task<IReceivedCommand> ReadAsync(CancellationToken token);

        /// <summary>
        /// Checks if the communication port is open
        /// </summary>
        /// <returns><code>true</code> if the port is open</returns>
        bool IsOpen();
    }
}
