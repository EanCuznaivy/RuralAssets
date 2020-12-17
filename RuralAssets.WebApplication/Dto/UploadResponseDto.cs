using System.Text.Json.Serialization;

namespace RuralAssets.WebApplication
{
    public class UploadResponseDto
    {
        [JsonPropertyName("code")] public string Code { get; set; }

        [JsonPropertyName("msg")] public string Msg { get; set; }

        [JsonPropertyName("result")] public string Result { get; set; }

        [JsonPropertyName("description")] public string Description { get; set; }

        [JsonPropertyName("file_id")] public string FileId { get; set; }

        [JsonPropertyName("file_hash")] public string FileHash { get; set; }

        [JsonPropertyName("transaction_id")] public string TransactionId { get; set; }
    }
}