using System.Text.Json.Serialization;

namespace RuralAssets.WebApplication
{
    public class DetailsInput
    {
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("idcard")] public string Idcard { get; set; }
        [JsonPropertyName("asset_type")] public string AssetType { get; set; }
        [JsonPropertyName("asset_id")] public string AssetId { get; set; }
    }
}