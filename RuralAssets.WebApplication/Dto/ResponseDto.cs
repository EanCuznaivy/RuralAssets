using System.Text.Json.Serialization;

namespace RuralAssets.WebApplication
{
    public class ResponseDto
    {
        [JsonPropertyName("code")] public string Code { get; set; }

        [JsonPropertyName("msg")] public string Msg { get; set; }

        [JsonPropertyName("result")] public string Result { get; set; }

        [JsonPropertyName("description")] public string Description { get; set; }
    }
}