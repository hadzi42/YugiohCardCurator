using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YugiohCardCurator.DTOs
{
    internal sealed class PriceData
    {
        [JsonPropertyName("listings")]
        public List<object> Listings { get; set; }

        [JsonPropertyName("prices")]
        public Price Prices { get; set; }
    }
}
