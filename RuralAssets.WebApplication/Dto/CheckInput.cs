using System.Text.Json.Serialization;

namespace RuralAssets.WebApplication
{
    public class CheckInput
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("idcard")]
        public string IdCard { get; set; }

        [JsonPropertyName("asset_type")]
        public string AssetType { get; set; }

        [JsonPropertyName("year")]
        public string Year { get; set; }
    }
}