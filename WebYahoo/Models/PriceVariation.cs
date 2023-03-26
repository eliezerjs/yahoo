namespace WebYahoo.Models
{
    public class PriceVariation
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public double DailyVariation { get; set; }
        public double PercentualVariation { get; set; }
        public string Ticker { get; set; }
    }
}
