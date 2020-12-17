using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RuralAssets.WebApplication
{
    public class UploadInput
    {
        [JsonPropertyName("file_type")] public string FileType { get; set; }
        [JsonPropertyName("idcard")] public string IdCard { get; set; }
        [JsonPropertyName("loan_id")] public string LoanId { get; set; }
        
        [JsonPropertyName("asset_id")] public int AssetId { get; set; }
        
        [JsonPropertyName("asset_type")] public int AssetType { get; set; }

        [JsonPropertyName("loan_file")]
        
        public IFormFile LoanFile { get; set; }
    }

    public class FileSavedInfo
    {
        public string FileId { get; set; }
        public string FileHash { get; set; }
    }
}