using System.Text;
using TelnetServer.Comm;

namespace TelnetServer.Telnet
{
    public abstract class CommandBase : IData
    {
        /// <summary>
        /// Interpret as Command escape character
        /// </summary>
        protected const char IAC = (char) 255;

        #region Implementation of IData
        byte[] IData.GetBytes() => Encoding.ASCII.GetBytes(this.GetCommandString());
        #endregion Implementation of IData

        public abstract string GetCommandString();
    }
}
