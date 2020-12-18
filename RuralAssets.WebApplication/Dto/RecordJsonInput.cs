using System.Text.Json.Serialization;

namespace RuralAssets.WebApplication
{
    public class RecordJsonInput
    {
        [JsonPropertyName("key")] public string Key { get; set; }
        [JsonPropertyName("json_message")] public string JsonMessage { get; set; }
    }
}