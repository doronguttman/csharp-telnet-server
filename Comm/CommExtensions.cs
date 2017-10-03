using System.Net.Sockets;

namespace TelnetServer.Comm
{
    internal static class CommExtensions
    {
        public static bool IsConnected(this TcpClient client) => client != null && client.Connected && client.Client.IsConnected();

        public static bool IsConnected(this Socket socket)
        {
            try
            {
                if (socket == null || !socket.Connected) return false;
                if (!socket.Poll(0, SelectMode.SelectRead)) return true;

                var buffer = new byte[1];
                return socket.Receive(buffer, SocketFlags.Peek) != 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
