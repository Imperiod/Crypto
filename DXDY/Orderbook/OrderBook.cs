using Newtonsoft.Json;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;

namespace DXDY.Orderbooks
{
    public class OrderBook
    {
        string type = "subscribe";
        string channel = "v3_orderbook";
        CancellationToken cancellationToken;
        WebSocket webSocket;
        public IList<OrderBookItem> Items { get; private set; }

        public OrderBook(WebSocket webSocket, CancellationToken cancellationToken)
        {
            this.webSocket = webSocket;
            this.cancellationToken = cancellationToken;
            Items = new List<OrderBookItem>();
        }

        public async Task Load(string CurrencyPair)
        {
            byte[] buffer;
            JsonSerializer serializer = new JsonSerializer();
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            var msg = new { channel = channel, type = type, id = CurrencyPair };

            serializer.Serialize(sw, msg);

            buffer = Encoding.UTF8.GetBytes(sw.ToString());


            await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken);
            Items = await Get();
        }

        private async Task<IList<OrderBookItem>> Get()
        {
            List<byte[]> data = new List<byte[]>();
            var result = new List<OrderBookItem>();
            var serializer = new JsonSerializer();

            while (true)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192*4]);

                var response = await webSocket.ReceiveAsync(buffer, cancellationToken);
                if (response.EndOfMessage)
                {
                    data.Add(buffer.ToArray());
                    foreach (var itemByte in data)
                    {
                        var _string = Encoding.UTF8.GetString(itemByte);
                        var sr = new StringReader(_string);
                        var reader = new JsonTextReader(sr);
                        serializer.Converters.Add(new OrderBookItemJsonConverter());

                        var item = serializer.Deserialize<OrderBookItem>(reader);
                        if (item is not null)
                        {
                            result.Add(item);
                        }
                    }

                    return result;
                }
                else
                {
                    data.Add(buffer.ToArray());
                }
            }
        }
    }
}