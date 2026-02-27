using System.Text.Json.Serialization;

namespace Aban360.BlobPool.Domain.Providers.Dto
{
    public class FileListResponse
    {
        [JsonPropertyName("document")]
        public List<Document> Documents { get; set; }
    }
    public class FileListResponseSingle
    {
        [JsonPropertyName("document")]
        public Document Document { get; set; }
    }

    public class Document
    {
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("permissions")]
        public int Permissions { get; set; }

        [JsonPropertyName("subscribed")]
        public bool Subscribed { get; set; }

        [JsonPropertyName("tenantId")]
        public int TenantId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        [JsonPropertyName("actualVersion")]
        public ActualVersion ActualVersion { get; set; }

        [JsonPropertyName("checkedOut")]
        public bool CheckedOut { get; set; }

        [JsonPropertyName("convertibleToPdf")]
        public bool ConvertibleToPdf { get; set; }

        [JsonPropertyName("convertibleToSwf")]
        public bool ConvertibleToSwf { get; set; }

        [JsonPropertyName("lastModified")]
        public DateTime LastModified { get; set; }

        [JsonPropertyName("locked")]
        public bool Locked { get; set; }

        [JsonPropertyName("mimeType")]
        public string MimeType { get; set; }

        [JsonPropertyName("signed")]
        public bool Signed { get; set; }
    }

    public class ActualVersion
    {
        [JsonPropertyName("actual")]
        public bool Actual { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("checksum")]
        public string Checksum { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("name")]
        public int Name { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }
    }

}
