using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace TelnetServer.Comm
{
    internal class Server : IDisposable
    {
        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();
        private readonly TcpListener _listener;
        private Task _listeningTask;

        public event EventHandler<Session> NewSession;

        public Server(int port)
        {
            this._listener = new TcpListener(IPAddress.Any, port);
        }

        #region IDisposable
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this._cancellation?.Dispose();
            this._listeningTask?.Dispose();
        }
        #endregion IDisposable

        public bool Start()
        {
            if (this._cancellation.IsCancellationRequested) throw new OperationCanceledException("Server is already cancelled", this._cancellation.Token);

            try
            {
                this._listener.Start();
                this._listeningTask = Task.Factory.StartNew(this.Listen, this._cancellation.Token, TaskCreationOptions.LongRunning);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Stop()
        {
            try
            {
                this._cancellation.Cancel();
                this._listeningTask.Wait();
                this._listener.Stop();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async void Listen(object @null)
        {
            try
            {
                while (true)
                {
                    var client = await Task.Factory.StartNew(() => this._listener.AcceptTcpClient(), this._cancellation.Token);
                    var session = new Session(client, CancellationTokenSource.CreateLinkedTokenSource(this._cancellation.Token).Token);
                    this.NewSession?.Invoke(this, session);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
