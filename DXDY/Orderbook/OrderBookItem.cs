using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXDY.Orderbooks
{
    public class OrderBookItem
    {
        public string Couple { get; init; }
        public IList<PublicOrder> Asks { get; init; }
        public IList<PublicOrder> Bids { get; init; }

        public OrderBookItem(string couple, IList<PublicOrder> asks, IList<PublicOrder> bids)
        {
            Couple = couple;
            Asks = asks;
            Bids = bids;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("====================================================================================================================");
            sb.AppendLine($"{nameof(Couple)}: {Couple}");
            sb.AppendLine($"{Environment.NewLine + nameof(Asks)}: {string.Join("", Asks.Select(s => Environment.NewLine + s.ToString()))}");
            sb.AppendLine($"{Environment.NewLine + nameof(Bids)}: {string.Join("", Bids.Select(s => Environment.NewLine + s.ToString()))}");
            sb.AppendLine("====================================================================================================================");

            return sb.ToString();
        }
    }

    public class OrderBookItemJsonConverter : JsonConverter<OrderBookItem>
    {
        public override OrderBookItem? ReadJson(JsonReader reader, Type objectType, OrderBookItem? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string? property = null;
            string couple = "";
            IList<PublicOrder> asks = new List<PublicOrder>();
            IList<PublicOrder> bids = new List<PublicOrder>();

            while (reader.Read())
            {
                if (property is null)
                {
                    switch (reader.TokenType)
                    {
                        case JsonToken.String:
                            break;
                        case JsonToken.Boolean:
                            break;
                        case JsonToken.PropertyName:
                            property = reader.Value?.ToString();
                            break;
                    }
                }
                else
                {
                    if(property == "contents")
                    {
                        string prop = "";
                        while(reader.Read())
                        {
                            if (reader.TokenType == JsonToken.PropertyName && prop == "")
                            {
                                prop = reader.Value?.ToString() ?? "";
                            }
                            if (reader.TokenType == JsonToken.StartArray && prop != "")
                            {
                                while (true)
                                {
                                    decimal price = default;
                                    decimal size = default;
                                    reader.Read();
                                    if (reader.TokenType == JsonToken.EndArray)
                                    {
                                        prop = "";
                                        break;
                                    }
                                    reader.Read();
                                    if (prop == "asks")
                                    {
                                        reader.Read();
                                        size = decimal.Parse((reader.Value?.ToString() ?? "0").Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
                                        reader.Read();
                                        reader.Read();
                                        price = decimal.Parse((reader.Value?.ToString() ?? "0").Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
                                        asks.Add(new PublicOrder(price, size));
                                        reader.Read();
                                    }
                                    else
                                    {
                                        reader.Read();
                                        size = decimal.Parse((reader.Value?.ToString() ?? "0").Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
                                        reader.Read();
                                        reader.Read();
                                        price = decimal.Parse((reader.Value?.ToString() ?? "0").Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
                                        bids.Add(new PublicOrder(price, size));
                                        reader.Read();
                                    }
                                }
                            }
                        }
                    }
                    if (property is not null && property == "id")
                    {
                        couple = reader.Value?.ToString() ?? "";
                    }
                    property = null;
                }

            }
            return new OrderBookItem(couple, asks, bids);

        }

        public override void WriteJson(JsonWriter writer, OrderBookItem? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
