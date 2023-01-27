namespace RichillCapital.Max.Models
{
    public class OHLC
    {
        public int Timestamp { get; internal set; }
        public double Open { get; internal set; }
        public double High { get; internal set; }
        public double Low { get; internal set; }
        public double Close { get; internal set; }
        public double Volume { get; internal set; }
    }
}
