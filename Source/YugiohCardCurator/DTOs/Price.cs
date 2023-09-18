using System.Text.Json.Serialization;

namespace YugiohCardCurator.DTOs
{
    internal sealed class Price
    {
        [JsonPropertyName("average")]
        public double Average { get; set; }

        public double high { get; set; }
        public double low { get; set; }
        public double shift { get; set; }
        public double shift_3 { get; set; }
        public double shift_7 { get; set; }
        public double shift_21 { get; set; }
        public double shift_30 { get; set; }
        public double shift_90 { get; set; }
        public double shift_180 { get; set; }
        public double shift_365 { get; set; }
        public string updated_at { get; set; }
    }
}
