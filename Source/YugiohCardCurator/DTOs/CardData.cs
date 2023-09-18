using System.Text.Json.Serialization;

namespace YugiohCardCurator.DTOs
{
    internal sealed class CardData
    {
        [JsonPropertyName("price_data")]
        public PrintTag PriceData { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("card_type")]
        public string CardType { get; set; }

        [JsonPropertyName("family")]
        public string Attribute { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("atk")]
        public int Atk { get; set; }

        [JsonPropertyName("def")]
        public int Def { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("property")]
        public object Property { get; set; }
    }
}
