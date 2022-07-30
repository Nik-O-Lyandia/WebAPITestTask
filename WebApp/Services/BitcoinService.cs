/*
 * BitcoinService provides all logic for BitcoinController
 */

using System.Text.Json;
using WebApp.Models;
using WebApp.Interfaces;

namespace WebApp.Services
{
    public class BitcoinService : IBitcoinService
    {
        private readonly IHttpService _httpService;

        public BitcoinService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public decimal Rate()
        {
            //Getting response from third-party API
            string response = _httpService.GetResponse();

            // Third-party API provides data in JSON, so we're deserializing it
            var currencies = JsonSerializer.Deserialize<List<Currency>>(response);

            if (currencies == null) return 0;

            return currencies.Single(c => c.Code == "UAH").Rate;
        }
    }
}
