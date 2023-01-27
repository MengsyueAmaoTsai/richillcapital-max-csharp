namespace RichillCapital.Max
{
    public class MaxTradingClient : MaxClient
    {
        public MaxTradingClient(string apiKey, string secretKey) 
            : base(apiKey, secretKey)
        { 
        }

        #region WebSocket API

        public void Authenticate()
        {

        }

        #endregion
    }
}
