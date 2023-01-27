using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RichillCapital.Max.Models;
using System.Text;

namespace RichillCapital.Max
{
    public class MaxMarketDataClient : MaxClient
    {
        public MaxMarketDataClient(string apiKey, string secretKey) 
            : base(apiKey, secretKey)
        { 
        }

        #region Market Data REST API Public
        
        /// <summary>
        /// Get OHLC of a specific market
        /// </summary>
        /// <param name="market"></param>
        /// <param name="period"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<OHLC>> GetOHLCAsync(string market, int period = 1, int limit = 30)
        {
            List<OHLC> ohlcList = new List<OHLC>();
            
            int[] supportsPeriods = new int[] { 1, 5, 15, 30, 60, 120, 240, 360, 720, 1440, 4320, 10080 };
            
            if (!supportsPeriods.Contains(period))
                throw new ArgumentException(nameof(period));

            string endpoint = $"/api/v2/k";
            var parameters = new JObject
            {
                { "market", market },
                { "limit", limit },
                { "period", period },
            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            string data = await response.Content.ReadAsStringAsync();

            JArray jsonArray = JArray.Parse(data);

            foreach (var ohlcData in jsonArray)
            {
                OHLC ohlc = new OHLC 
                {
                    Timestamp = ohlcData[0].Value<int>(),
                    Open = ohlcData[1].Value<double>(),
                    High = ohlcData[2].Value<double>(),
                    Low = ohlcData[3].Value<double>(),
                    Close = ohlcData[4].Value<double>(),
                    Volume = ohlcData[5].Value<double>(),
                };
                ohlcList.Add(ohlc);
            }
            return ohlcList;
        }

        #endregion

        #region Websocket API

        public void SubscribeMarketTrade(string market)
        {
        }

        public void UnsubscribeMarketTrade(string market)
        {
        }

        public void SubscribeOrderBook(string market)
        {
        }

        public void UnsubscribeOrderBook(string market)
        {
        }

        public void SubscribeTicker()
        {
        }

        public void UnsubscribeTicker()
        {
        }

        public void SubscribeMarketStatus()
        {
        }

        public void UnsubscribeMarketStatus()
        {

        }

        #endregion
    }
}
