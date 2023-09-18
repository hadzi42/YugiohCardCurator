using System.Text.Json.Serialization;

namespace YugiohCardCurator.DTOs
{
    internal sealed class PriceDataResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("data")]
        public PriceData Data { get; set; }
    }
}
