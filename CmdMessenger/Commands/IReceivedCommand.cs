
namespace CmdMessenger.Commands
{
    public interface IReceivedCommand
    {
        /// <summary>
        /// Gets the command ID.
        /// </summary>
        int CommandId { get; }

        /// <summary>
        /// Read <code>short</code>
        /// </summary>
        /// <returns>The <code>short</code> value.</returns>
        short ReadInt16();

        /// <summary>
        /// Read <code>bool</code>
        /// </summary>
        /// <returns>The <code>bool</code> value.</returns>
        bool ReadBool();

        /// <summary>
        /// Read <code>int</code>
        /// </summary>
        /// <returns>The <code>int</code> value.</returns>
        int ReadInt32();

        /// <summary>
        /// Read <code>UInt32</code>
        /// </summary>
        /// <returns>The <code>uint</code> value.</returns>
        uint ReadUInt32();

        /// <summary>
        /// Read the current argument as string.
        /// </summary>
        /// <returns>The <code>sting</code> value.</returns>
        string ReadString();
    }
}
