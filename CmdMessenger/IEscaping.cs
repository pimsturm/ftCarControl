using System.Collections.Generic;
using System.IO;

namespace CmdMessenger
{
    public interface IEscaping
    {
        /// <summary>
        /// Get the un-escaped parameters  
        /// </summary>
        /// <param name="line">
        /// The line to parse.
        /// </param>
        /// <returns>
        /// A collection of the unescaped parameters.
        /// </returns>
        string[] GetUnescapedParameters(byte[] line);

        /// <summary>
        /// Pack the message ready for sending.
        /// </summary>
        /// <param name="input">The parameters to escape.</param>
        /// <returns>The escaped parameters.</returns>
        string EscapeParameters(params string[] input);

        /// <summary>
        /// Read off the first command.
        /// </summary>
        /// <param name="temp">
        /// The string containing multiple commands.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// A enumerable of commands.
        /// </returns>
        IEnumerable<string> GetCommands(string temp);

        IEnumerable<byte[]> GetCommand(Stream stream);
    }
}
