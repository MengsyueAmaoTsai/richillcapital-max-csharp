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

        public void SubscribeTrade()
        {

        }
        
        public void UnsubscribeTrade()
        {

        }
 
        public void SubscribeOrder()
        {

        }   
        
        public void UnsubscribeOrder()
        {

        }
        
        public void SubscribeAccount()
        {

        }

        public void UnsubscribeAccount()
        {

        }

        #endregion
    }
}
