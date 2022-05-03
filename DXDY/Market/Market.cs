using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace DXDY.Markets
{
    public class Market
    {
        string type = "subscribe";
        string channel = "v3_markets";
        CancellationToken cancellationToken;
        WebSocket webSocket;
        public IList<MarketItem> Items { get; private set; }

        public Market(WebSocket webSocket, CancellationToken cancellationToken)
        {
            this.webSocket = webSocket;
            this.cancellationToken = cancellationToken;
            Items = new List<MarketItem>();
        }

        public async Task Load()
        {
            byte[] buffer;
            JsonSerializer serializer = new JsonSerializer();
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            var msg = new { channel = channel, type = type };

            serializer.Serialize(sw, msg);

            buffer = Encoding.UTF8.GetBytes(sw.ToString());


            await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken);
            Items = await Get();
        }

        private async Task<IList<MarketItem>> Get()
        {
            var result = new List<MarketItem>();
            var serializer = new JsonSerializer();

            while (true)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192 * 100]);

                var response = await webSocket.ReceiveAsync(buffer, cancellationToken);

                var _string = Encoding.UTF8.GetString(buffer);
                var sr = new StringReader(_string);
                var reader = new JsonTextReader(sr);
                serializer.Converters.Add(new MarketJsonConverter());

                var item = serializer.Deserialize<IList<MarketItem>>(reader);
                if (item is not null)
                {
                    result.AddRange(item);
                }

                return result;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("====================================================================================================================");
            sb.AppendLine("AVAILABLE MARKETS:");
            sb.AppendLine(string.Join("", Items.Select(x => x.ToString() + Environment.NewLine)));
            sb.AppendLine("====================================================================================================================");

            return sb.ToString();
        }
    }

    public class MarketJsonConverter : JsonConverter<IList<MarketItem>>
    {
        public override IList<MarketItem>? ReadJson(JsonReader reader, Type objectType, IList<MarketItem>? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            List<MarketItem> result = new List<MarketItem>();
            string? property = null;
            string market = "";
            MarketStatus status = MarketStatus.OFFLINE;
            string baseAsset = "";
            string quoteAsset = "";
            decimal stepSize = 0;
            decimal tickSize = 0;
            decimal indexPrice = 0;
            decimal oraclePrice = 0;
            decimal priceChange24H = 0;
            decimal nextFundingRate = 0;
            DateTimeOffset nextFundingAt = DateTimeOffset.MinValue;
            decimal minOrderSize = 0;
            decimal initialMarginFraction = 0;
            decimal maintenanceMarginFraction = 0;
            decimal baselinePositionSize = 0;
            decimal incrementalPositionSize = 0;
            decimal incrementalInitialMarginFraction = 0;
            decimal volume24H = 0;
            decimal trades24H = 0;
            decimal openInterest = 0;
            decimal maxPositionSize = 0;
            decimal assetResolution = 0;
            string syntheticAssetId = "";

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    property = reader.Value?.ToString();
                }
                if (property == "contents")
                {
                    property = null;
                    reader.Read();
                    reader.Read();
                    reader.Read();
                    reader.Read();
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.EndObject)
                        {
                            break;
                        }
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            while (reader.TokenType != JsonToken.EndObject)
                            {
                                reader.Read();
                                if (reader.TokenType == JsonToken.PropertyName && property is null)
                                {
                                    property = reader.Value?.ToString();
                                }
                                else
                                {
                                    switch (property)
                                    {
                                        case "market": market = reader.Value?.ToString() ?? ""; break;
                                        case "status": status = (MarketStatus)Enum.Parse(typeof(MarketStatus), reader.Value?.ToString() ?? ""); break;
                                        case "baseAsset": baseAsset = reader.Value?.ToString() ?? ""; break;
                                        case "quoteAsset": quoteAsset = reader.Value?.ToString() ?? ""; break;
                                        case "stepSize": stepSize = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "tickSize": tickSize = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "indexPrice": indexPrice = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "oraclePrice": oraclePrice = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "priceChange24H": priceChange24H = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "nextFundingRate": nextFundingRate = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "nextFundingAt": nextFundingAt = DateTimeOffset.Parse(reader.Value?.ToString() ?? DateTimeOffset.MinValue.ToString()); break;
                                        case "minOrderSize": minOrderSize = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "initialMarginFraction": initialMarginFraction = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "maintenanceMarginFraction": maintenanceMarginFraction = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "baselinePositionSize": baselinePositionSize = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "incrementalPositionSize": incrementalPositionSize = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "incrementalInitialMarginFraction": incrementalInitialMarginFraction = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "volume24H": volume24H = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "trades24H": trades24H = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "openInterest": openInterest = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "maxPositionSize": maxPositionSize = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "assetResolution": assetResolution = decimal.Parse(reader.Value?.ToString()?.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) ?? ""); break;
                                        case "syntheticAssetId": syntheticAssetId = reader.Value?.ToString() ?? ""; break;
                                    }
                                    property = null;
                                }
                            }
                        }
                        if (reader.TokenType == JsonToken.EndObject)
                        {

                            result.Add(new MarketItem(market, status, baseAsset, quoteAsset, stepSize, tickSize, indexPrice, oraclePrice, priceChange24H
                                , nextFundingRate, nextFundingAt, minOrderSize, initialMarginFraction, maintenanceMarginFraction, baselinePositionSize, incrementalPositionSize,
                                incrementalInitialMarginFraction, volume24H, trades24H, openInterest, maxPositionSize, assetResolution, syntheticAssetId));
                            reader.Read();
                        }
                    }
                }

            }


            return result;
        }

        public override void WriteJson(JsonWriter writer, IList<MarketItem>? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
