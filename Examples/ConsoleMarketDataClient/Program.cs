
using RichillCapital.Max;
using RichillCapital.Max.Models;

string apiKey = "TC90Qp8MMiexoBXe6WPGfoKZItWdfRQSy3vnKXiJ";
string secretKey = "yhf0JAIa8xY0UwcTFTSw7xAeLzDUs72mxwVJlSk0";

MaxMarketDataClient client = new MaxMarketDataClient(apiKey, secretKey);

// Get server time
int serverTimestamp = await client.GetServerTimeAsync();
Console.WriteLine($"Server timestamp: {serverTimestamp}");

// Get vip Levels
List<VipLevel> vipLevels = await client.GetVipLevelsAsync();
vipLevels.ForEach(vipLevel => Console.WriteLine($"Level: {vipLevel.Level} MakerFee: {vipLevel.MakerFee} TakerFee: {vipLevel.TakerFee}"));

// Get OHLC
List<OHLC> ohlcs = await client.GetOHLCAsync("btctwd");
ohlcs.ForEach(ohlc => Console.WriteLine($"Timestamp: {ohlc.Timestamp} Open: {ohlc.Open} High: {ohlc.High} Low: {ohlc.Low} Close: {ohlc.Close} Volume: {ohlc.Volume}"));


// Get profile
await client.GetUserProfileAsync();


Console.ReadKey();
