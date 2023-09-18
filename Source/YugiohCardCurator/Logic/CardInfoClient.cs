using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using YugiohCardCurator.DTOs;

namespace YugiohCardCurator.Logic
{
    internal sealed class CardInfoClient : IDisposable
    {
        private static readonly Uri _BaseAddress = new Uri("https://yugiohprices.com/api/");
        private HttpClient _httpClient;

        public CardInfoClient()
        {
            _httpClient = new HttpClient { BaseAddress = _BaseAddress };
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
            _httpClient = null;
        }

        public async Task<CardData> GetPriceByPrintTagAsync(string printTag)
        {
            using (HttpResponseMessage response = await _httpClient.GetAsync("price_for_print_tag/" + printTag).ConfigureAwait(false))
            {
                string responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                PriceResponse priceResponse = JsonSerializer.Deserialize<PriceResponse>(responseData);
                return priceResponse.Data;
            }
        }

        public async Task<CardData> GetCardDataAsync(string name)
        {
            using (HttpResponseMessage response = await _httpClient.GetAsync("card_data/" + name).ConfigureAwait(false))
            {
                string responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonSerializer.Deserialize<PriceResponse>(responseData).Data;
            }
        }
    }
}
