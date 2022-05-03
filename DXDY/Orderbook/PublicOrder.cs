using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXDY.Orderbooks
{
    public class PublicOrder
    {
        public decimal Price { get; init; }
        public decimal Size { get; init; }

        public PublicOrder(decimal price, decimal size)
        {
            Price = price;
            Size = size;
        }

        public string ToJsonString()
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            jsonSerializer.Serialize(writer, this);
            writer.Flush();
            return sw.ToString();
        }

        public override string ToString()
        {
            return $"[{nameof(Price)}: {Price}; {nameof(Size)}: {Size}]";
        }
    }
}
