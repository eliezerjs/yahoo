using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StockService.Data;
using WebYahoo.Models;

namespace StockInfoService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockInfoController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly StockDbContext _context;
        public StockInfoController(HttpClient client, StockDbContext context)
        {
            _client = client;
            _context = context;
        }

        [HttpGet("history/{ticker}")]
        public IActionResult GetPriceVariationHistory(string ticker)
        {
            var priceVariations = _context.PriceVariations.Where(pv => pv.Ticker == ticker).ToList();

            if (priceVariations.Count == 0)
            {
                return NotFound();
            }

            var formattedPriceVariations = priceVariations.Select(pv => new
            {
                pv.Day,
                Date = pv.Date.ToString("dd/MM/yyyy"),
                Value = pv.Value.ToString("N2", CultureInfo.InvariantCulture),
                DailyVariation = pv.DailyVariation.ToString("N2", CultureInfo.InvariantCulture) + "%",
                PercentualVariation = pv.PercentualVariation.ToString("N2", CultureInfo.InvariantCulture) + "%",
                pv.Ticker
            });

            return Ok(formattedPriceVariations);
        }

        [HttpPost("{ticker}")]
        public async Task<IActionResult> SearchAndSaveVariations(string ticker)
        {
            var response = await _client.GetAsync($"https://query2.finance.yahoo.com/v8/finance/chart/{ticker}?interval=1d&range=1mo&includePrePost=true");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(jsonString);

            var chart = json["chart"];
            var result = chart["result"][0];
            var indicators = result["indicators"];
            var quote = indicators["quote"][0];
            var open = quote["open"].ToList().Select(x => x.ToString());
            var openPricesWhere = open.Where(t => t != string.Empty);
            var openPrices = openPricesWhere.Select(t => Convert.ToDouble(t)).ToList();

            if (openPrices.Count == 0)
            {
                return NotFound();
            }

            double firstOpenPrice = openPrices[0];
            double lastOpenPrice = openPrices[openPrices.Count - 1];
            double priceVariation = lastOpenPrice - firstOpenPrice;

            var timestamps = json["chart"]["result"][0]["timestamp"].Select(t => DateTimeOffset.FromUnixTimeSeconds((long)t).DateTime).ToList();

            if (openPrices.Count == 0)
            {
                return NotFound();
            }

            var last30DaysData = openPrices.Zip(timestamps, (price, date) => new { Price = price, Date = date }).TakeLast(30).ToList();

            double previousDayPrice = last30DaysData[0].Price;

            var priceVariations = new List<PriceVariation>();

            if(_context.PriceVariations.Any(x => x.Ticker == ticker))
            {
                _context.PriceVariations.RemoveRange(_context.PriceVariations.Where(x => x.Ticker == ticker));
            }

            for (int i = 1; i < last30DaysData.Count; i++)
            {
                var dayData = last30DaysData[i];
                double dailyVariation = dayData.Price - previousDayPrice;
                double accumulatedVariation = dayData.Price - last30DaysData[0].Price;

                var priceVariationItem = new PriceVariation
                {
                    Day = i,
                    Date = dayData.Date,
                    Value = dayData.Price,
                    DailyVariation = dailyVariation,
                    PercentualVariation = (accumulatedVariation / firstOpenPrice) * 100, // Cálculo do percentual de variação em relação ao primeiro dia
                    
                    Ticker = ticker
                };

                priceVariations.Add(priceVariationItem);

                previousDayPrice = dayData.Price;
            }
            _context.PriceVariations.AddRange(priceVariations);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
