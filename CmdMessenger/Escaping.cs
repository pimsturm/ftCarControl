using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CmdMessenger
{
    public class Escaping : IEscaping
    {
        #region Fields

        /// <summary>
        /// Gets the fields separator.
        /// </summary>
        private readonly char fieldSeparator;

        /// <summary>
        /// Gets the command separator.
        /// </summary>
        private readonly char commandSeparator;

        /// <summary>
        /// Gets the escape character.
        /// </summary>
        private readonly char escapeCharacter;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Escaping"/> class.
        /// </summary>
        /// <param name="fieldSeparator">The fields separator.</param> 
        /// <param name="commandSeparator">The command separator.</param>
        /// <param name="escapeCharacter">The escape character.</param>
        public Escaping(char fieldSeparator, char commandSeparator, char escapeCharacter) {
            this.fieldSeparator = fieldSeparator;
            this.commandSeparator = commandSeparator;
            this.escapeCharacter = escapeCharacter;
        }

        public static IEscaping Default {
            get { return new Escaping(',', ';', '/'); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the un-escaped parameters  
        /// </summary>
        /// <param name="line">
        /// The line to parse.
        /// </param>
        /// <returns>
        /// A collection of unescaped parameters.
        /// </returns>
        public string[] GetUnescapedParameters(byte[] line) {
            byte[] tempLine = line;

            var parameters = new List<string>();
            var sb = new StringBuilder();
            bool escapeMode = false;

            foreach (char t in tempLine) {
                if (escapeMode) {
                    sb.Append(t);
                    escapeMode = false;
                    continue;
                }

                if (t == this.escapeCharacter) {
                    escapeMode = true;
                    continue;
                }

                if (t == this.fieldSeparator) {
                    parameters.Add(sb.ToString());
                    sb.Clear();
                    continue;
                }

                sb.Append(t);
            }

            if (sb.Length > 0) {
                parameters.Add(sb.ToString());
            }

            return parameters.ToArray();
        }

        /// <summary>
        /// Pack the message ready for sending.
        /// </summary>
        /// <param name="input">The parameters to escape.</param>
        /// <returns>The escaped parameters.</returns>
        public string EscapeParameters(params string[] input) {
            var parameters = new List<string>();

            foreach (var item in input) {
                string temp = item;
                if (item.Contains(this.fieldSeparator)) {
                    temp = temp.Replace(
                        this.fieldSeparator.ToString(),
                        string.Format("{0}{1}", this.escapeCharacter, this.fieldSeparator));
                }

                parameters.Add(temp);
            }

            return string.Concat(string.Join(this.fieldSeparator.ToString(), parameters), this.commandSeparator);
        }

        /// <summary>
        /// Read off the first command.
        /// </summary>
        /// <param name="temp">
        /// The the commands from the string.
        /// </param>
        /// <returns>
        /// The <see cref="Enumerable"/>.
        /// An enumeration of the commands.
        /// </returns>
        public IEnumerable<string> GetCommands(string temp) {
            var commands = new List<string>();
            var sb = new StringBuilder();
            bool escapeMode = false;

            foreach (char t in temp) {
                if (escapeMode) {
                    sb.Append(t);
                    escapeMode = false;
                    continue;
                }

                if (t == this.escapeCharacter) {
                    escapeMode = true;
                    continue;
                }

                if (t == this.commandSeparator) {
                    commands.Add(sb.ToString());
                    sb.Clear();
                    continue;
                }

                sb.Append(t);
            }

            return commands.ToArray();
        }

        public IEnumerable<byte[]> GetCommand(Stream stream) {
            var sb = new List<byte>();
            bool escapeMode = false;

            int read = 0;

            while (read != -1) {
                read = stream.ReadByte();

                if (escapeMode) {
                    sb.Add((byte)read);
                    escapeMode = false;
                    continue;
                }

                if (read == this.escapeCharacter) {
                    escapeMode = true;
                    continue;
                }

                if (read == this.commandSeparator) {
                    yield return sb.ToArray();
                    sb.Clear();
                }

                sb.Add((byte)read);
            }
        }

        #endregion
    }
}
