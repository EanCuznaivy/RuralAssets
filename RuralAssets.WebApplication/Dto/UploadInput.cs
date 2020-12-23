using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RuralAssets.WebApplication
{
    public class UploadInput
    {
        [JsonPropertyName("file_type")] public string file_type { get; set; }
        [JsonPropertyName("idcard")] public string idcard { get; set; }
        [JsonPropertyName("loan_id")] public string loan_id { get; set; }

        [JsonPropertyName("asset_id")] public string asset_id { get; set; }

        [JsonPropertyName("asset_type")] public string asset_type { get; set; }

        [JsonPropertyName("file_hash")] public string file_hash { get; set; }

        [JsonPropertyName("loan_file")] public IFormFile loan_file { get; set; }
    }

    public class FileSavedInfo
    {
        public string FileId { get; set; }
        public string FileHash { get; set; }
    }
}