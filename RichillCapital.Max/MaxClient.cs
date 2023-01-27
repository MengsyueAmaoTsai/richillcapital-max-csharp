using System.Text;
using System.Security.Cryptography;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RichillCapital.Max.Models;
using RichillCapital.WebSockets;
using RichillCapital.WebSockets.Events;


namespace RichillCapital.Max
{
    public abstract class MaxClient
    {
        protected const string REST_URL = "https://max-api.maicoin.com";
        protected const string WEBSOCKET_URL = "wss://max-stream.maicoin.com/ws";

        protected HttpClient _httpClient;
        protected WebSocketClient _socketClient;

        protected string _apiKey;
        protected string _secretKey;

        protected MaxClient(string apiKey, string secretKey)
        {
            _apiKey = apiKey;
            _secretKey = secretKey;
            _httpClient = CreateHttpClient(REST_URL);
            _socketClient = CreateWebSocket(WEBSOCKET_URL);
        }

        #region REST API Public

        /// <summary>
        /// Get server current time, in seconds since Unix epoch.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetServerTimeAsync()
        {
            string endpoint = "/api/v2/timestamp";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            string data = await response.Content.ReadAsStringAsync();
            return Convert.ToInt32(data);
        }

        /// <summary>
        /// Get all vip level fees.
        /// </summary>
        /// <returns></returns>
        public async Task<List<VipLevel>> GetVipLevelsAsync()
        {
            string endpoint = $"/api/v2/vip_levels";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            HttpResponseMessage response = await _httpClient.SendAsync(request);    
            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<VipLevel>>(data);
        }

        #endregion

        #region REST API Private

        public async Task GetUserProfileAsync()
        {
            string endpoint = "/api/v2/members/me";
            long nonce = GetNonce();

            var parameters = new Dictionary<string, object>
            {
                { "nonce", nonce }
            };

            var parametersToSign = new Dictionary<string, object>(parameters)
            {
                { "path", endpoint }
            };

            string encodedPayload = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(parametersToSign)));
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
            byte[] signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(encodedPayload));
            string signature = BitConverter.ToString(signatureBytes).Replace("-", "").ToLower();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{endpoint}");
            request.Headers.Add("X-MAX-ACCESSKEY", _apiKey);
            request.Headers.Add("X-MAX-PAYLOAD", encodedPayload);
            request.Headers.Add("X-MAX-SIGNATURE", signature);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            //JObject json = JObject.Parse(await response.Content.ReadAsStringAsync());
            //Console.WriteLine($"{json.ToString()}");
        }

        #endregion

        #region WebSocket Event Handlers

        protected virtual void OnWebSocketMessage(object? sender, WebSocketMessageEventArgs e)
        {
        }

        protected virtual void OnWebSocketError(object? sender, WebSocketErrorEventArgs e)
        {
        }

        protected virtual void OnWebSocketClose(object? sender, WebSocketCloseEventArgs e)
        {
        }

        protected virtual void OnWebSocketOpen(object? sender, EventArgs e)
        {
        }

        #endregion

        /// <summary>
        /// Get nonce.
        /// </summary>
        /// <returns></returns>
        protected long GetNonce() => Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);

        /// <summary>
        /// Create a http client.
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <returns></returns>
        protected HttpClient CreateHttpClient(string baseAddress)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress),
            };
            return client;
        }

        /// <summary>
        /// Create a websocket client.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected WebSocketClient CreateWebSocket(string url)
        {
            WebSocketClient client = new WebSocketClient(url);
            client.OnOpen += OnWebSocketOpen;
            client.OnClose += OnWebSocketClose;
            client.OnError += OnWebSocketError;
            client.OnMessage += OnWebSocketMessage;
            return client;
        }
    }
}
