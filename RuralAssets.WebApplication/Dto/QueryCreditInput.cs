using System.Text.Json.Serialization;

namespace RuralAssets.WebApplication
{
    public class QueryCreditInput
    {
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("idcard")] public string IdCard { get; set; }
        [JsonPropertyName("asset_type")] public string AssetType { get; set; }
    }
}