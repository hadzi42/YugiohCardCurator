using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using YugiohCardCurator.DTOs;

namespace YugiohCardCurator.Logic
{
    internal sealed class CardInfoClient : IDisposable
    {
        private static readonly Uri _BaseAddress = new Uri("https://yugiohprices.com/api/");
        private readonly CardDataCache _Cache;
        private HttpClient _httpClient;

        public CardInfoClient()
        {
            _httpClient = new HttpClient { BaseAddress = _BaseAddress };
            _Cache = new CardDataCache();
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
            if (_Cache.TryGet(name, out CardData data))
                return data;

            string encoded = HttpUtility.UrlEncode(name);
            using (HttpResponseMessage response = await _httpClient.GetAsync("card_data/" + encoded).ConfigureAwait(false))
            {
                string responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                data = JsonSerializer.Deserialize<PriceResponse>(responseData).Data;
                _Cache.Add(name, data);
                return data;
            }
        }
    }
}
