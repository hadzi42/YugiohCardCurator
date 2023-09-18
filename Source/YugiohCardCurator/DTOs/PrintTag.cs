using System.Text.Json.Serialization;

namespace YugiohCardCurator.DTOs
{
    internal sealed class PrintTag
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("print_tag")]
        public string PrintTagName { get; set; }

        [JsonPropertyName("rarity")]
        public string Rarity { get; set; }

        [JsonPropertyName("price_data")]
        public PriceDataResponse PriceData { get; set; }
    }
}
