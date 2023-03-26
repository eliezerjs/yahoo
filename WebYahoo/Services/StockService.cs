using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockService.Services
{
    public class YahooFinanceService
    {
        private readonly HttpClient _client;

        public YahooFinanceService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetStockData(string ticker)
        {
            var url = $"https://query2.finance.yahoo.com/v8/finance/chart/{ticker}";
            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to retrieve data for ticker {ticker}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}