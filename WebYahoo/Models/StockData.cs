using YahooFinanceApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebYahoo.Models
{
    public class TradingPeriod
    {
        public string Timezone { get; set; }
        public long Start { get; set; }
        public long End { get; set; }
        public int Gmtoffset { get; set; }
    }

    public class CurrentTradingPeriod
    {
        public TradingPeriod Pre { get; set; }
        public TradingPeriod Regular { get; set; }
        public TradingPeriod Post { get; set; }
    }

    public class Meta
    {
        public string Currency { get; set; }
        public string Symbol { get; set; }
        public string ExchangeName { get; set; }
        public string InstrumentType { get; set; }
        public long FirstTradeDate { get; set; }
        public long RegularMarketTime { get; set; }
        public int Gmtoffset { get; set; }
        public string Timezone { get; set; }
        public string ExchangeTimezoneName { get; set; }
        public double RegularMarketPrice { get; set; }
        public double ChartPreviousClose { get; set; }
        public double PreviousClose { get; set; }
        public int Scale { get; set; }
        public int PriceHint { get; set; }
        public CurrentTradingPeriod CurrentTradingPeriod { get; set; }
        public List<List<TradingPeriod>> TradingPeriods { get; set; }
        public string DataGranularity { get; set; }
        public string Range { get; set; }
        public List<string> ValidRanges { get; set; }
    }
    public class Quote
    {
        public List<double?> Volume { get; set; }
        public List<double?> Low { get; set; }
        public List<double?> Close { get; set; }
        public List<double?> Open { get; set; }
        public List<double?> High { get; set; }
    }

    public class Indicators
    {
        [JsonProperty("quote")]
        public List<Quote> Quote { get; set; }
    }

    public class Result
    {
        public Meta Meta { get; set; }
        public List<long> Timestamp { get; set; }
        public Indicators Indicators { get; set; }
    }

    public class Chart
    {
        public List<Result> Result { get; set; }
        public object Error { get; set; }
    }

    public class Root
    {
        public Chart Chart { get; set; }
    }
}