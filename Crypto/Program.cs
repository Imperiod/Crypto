using DXDY;

API api = new API();
await api.Market.Load();
var items = api.Market.Items;
string couple = items.First(f => f.BaseAsset == "USD" || f.QuoteAsset == "USD").GetCouple();
await api.OrderBook.Load(couple);

Console.WriteLine(api.Market);
var item = api.OrderBook.Items.FirstOrDefault(f => f.Couple == "BTC-USD");

Console.WriteLine(item);
Console.ReadLine();