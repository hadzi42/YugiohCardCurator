using System.Text.Json.Serialization;

namespace YugiohCardCurator.DTOs
{
    internal sealed class PriceResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("data")]
        public CardData Data { get; set; }
    }
}
