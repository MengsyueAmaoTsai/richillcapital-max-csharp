using Newtonsoft.Json;


namespace RichillCapital.Max.Models
{
    public class VipLevel
    {
        [JsonProperty("level")]
        public int Level { get; internal set; }

        [JsonProperty("minimum_trading_volume")]
        public int MinimumTradingVolume { get; internal set; }

        [JsonProperty("minimum_staking_volume")]
        public int MinimumStakingVolume { get; internal set; }

        [JsonProperty("maker_fee")]
        public decimal MakerFee { get; internal set; }

        [JsonProperty("taker_fee")]
        public decimal TakerFee { get; internal set; }   
    }
}
