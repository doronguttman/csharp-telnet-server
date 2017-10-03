using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TelnetServer.Comm
{
    internal class Session : IDisposable
    {
        private readonly TcpClient _client;
        private readonly CancellationToken _cancellation;
        private readonly NetworkStream _stream;

        public event EventHandler<byte[]> BytesReceived;
        public event EventHandler<string> MessageReceived;
        public event EventHandler Disconnected;

        public Session(TcpClient client, CancellationToken cancellationToken)
        {
            this._client = client;
            this._cancellation = cancellationToken;
            this._stream = client.GetStream();
            Task.Factory.StartNew(this.Receive, this._cancellation, TaskCreationOptions.LongRunning);
        }

        #region IDisposable
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this._stream?.Dispose();
            this.Close();
            this._client?.Dispose();
            this.Disconnected?.Invoke(this, EventArgs.Empty);
        }
        #endregion IDisposable

        public async Task Send(byte[] bytes) => await this._stream.WriteAsync(bytes, 0, bytes.Length, this._cancellation);
        public async Task Send(string message) => await this.Send(Encoding.ASCII.GetBytes(message));
        public async Task Send(IData data) => await this.Send(data.GetBytes());

        private async void Receive(object @null)
        {
            const int bufferSize = 80 * 25;

            try
            {
                while (!this._cancellation.IsCancellationRequested)
                {
                    var buffer = new byte[bufferSize];
                    byte[] bytes;
                    using (var mem = new MemoryStream())
                    {
                        int bytesRead;
                        do
                        {
                            bytesRead = await this._stream.ReadAsync(buffer, 0, bufferSize, this._cancellation);
                            if (bytesRead == 0 && !this._client.IsConnected()) throw new OperationCanceledException("Disconnected");
                            await mem.WriteAsync(buffer, 0, bytesRead, this._cancellation);
                        } while (bytesRead == bufferSize);
                        bytes = mem.ToArray();
                    }

                    this.BytesReceived?.Invoke(this, bytes);

                    var message = Encoding.ASCII.GetString(bytes);
                    this.MessageReceived?.Invoke(this, message);
                }
            }
            catch (Exception)
            {
                this.Dispose();
            }
        }

        public void Close() => this._client.Close();

    }
}
