using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using DXDY.Markets;
using DXDY.Orderbooks;

namespace DXDY
{
    public class API
    {
        Uri uri = new Uri("wss://api.dydx.exchange/v3/ws");
        CancellationToken cancellationToken = new CancellationToken();
        ClientWebSocket clientWebSocket = new ClientWebSocket();

        public Market Market { get; private set; }
        public OrderBook OrderBook { get; private set; }

        public API()
        {
            Connect().Wait();
            OrderBook = new OrderBook(clientWebSocket, cancellationToken);
            Market = new Market(clientWebSocket, cancellationToken);
        }

        private async Task Connect()
        {
            await clientWebSocket.ConnectAsync(uri, cancellationToken);
            await Receive();
        }

        private async Task Receive()
        {
            WebSocketReceiveResult response;
            do
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);

                response = await clientWebSocket.ReceiveAsync(buffer, cancellationToken);
            } while (response.EndOfMessage == false);
        }
    }
}
