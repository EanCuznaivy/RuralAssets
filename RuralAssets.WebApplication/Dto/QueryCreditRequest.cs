using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RuralAssets.WebApplication
{
    public class QueryCreditRequest
    {
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("idcard")] public string Idcard { get; set; }
        [JsonPropertyName("asset_list")] public List<AssetRequest> AssetList { get; set; }
    }

    public class AssetRequest
    {
        [JsonPropertyName("asset_id")] public int AssetId { get; set; }
        [JsonPropertyName("bczje")] public double BCZJE { get; set; }
    }
}