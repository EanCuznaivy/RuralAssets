using System.Text.Json.Serialization;

namespace RuralAssets.WebApplication
{
    public class QueryCreditResponseDto
    {
        [JsonPropertyName("code")] public string Code { get; set; }
        [JsonPropertyName("msg")] public string Msg { get; set; }
        [JsonPropertyName("result")] public string Result { get; set; }
        [JsonPropertyName("credit")] public double Credit { get; set; }
    }
}