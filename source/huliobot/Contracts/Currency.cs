namespace huliobot.Contracts
{
    public class Money
    {
        public bool success { get; set; }
        public string terms { get; set; }
        public string privacy { get; set; }
        public int timestamp { get; set; }
        public string source { get; set; }
        public Quotes quotes { get; set; }
    }

    public class Quotes
    {
        public float USDUSD { get; set; }
        public float USDRUB { get; set; }
    }

}