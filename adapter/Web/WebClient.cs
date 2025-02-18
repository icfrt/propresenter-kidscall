using SocketIOClient;

namespace Icf.ProPresenter.KidsCall.Web
{
    internal class WebClient : IDisposable
    {
        private SocketIOClient.SocketIO client;

        public event EventHandler<EntryReceivedEventArgs>? EntryReceived;

        public WebClient(Uri url, string authorizationCode)
        {
            this.client = new SocketIOClient.SocketIO(url, new SocketIOOptions
            {
                Path = "/socket.io/",
                EIO = SocketIO.Core.EngineIO.V4
            });

            this.client.OnConnected += (_, e) =>
            {
                this.client.EmitAsync("authorize", new { code = authorizationCode });
            };

            this.client.OnError += (_, e) =>
            {
                Console.WriteLine(e.ToString());
            };

            this.client.OnReconnectFailed += (_, e) =>
            {
                //
            };

            this.client.On("authorized", response =>
            {
                this.Authorized = response.GetValue<bool>();
            });

            this.client.On("entry", response =>
            {
                var model = response.GetValue<EntryModel>();
                this.EntryReceived?.Invoke(this, new EntryReceivedEventArgs(model));
            });
        }

        public bool Authorized { get; private set; }

        public Task SetStateAsync(EntryModel model)
        {
            return this.client.EmitAsync("process", model);
        }

        public Task ConnectAsync()
        {
            return this.client.ConnectAsync();
        }

        public void Dispose()
        {
            this.client.Dispose();
        }
    }
}
