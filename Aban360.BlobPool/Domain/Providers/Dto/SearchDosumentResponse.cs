using System.Text.Json.Serialization;

namespace Aban360.BlobPool.Domain.Providers.Dto
{
    public class SearchDocumentsResponse
    {
        [JsonPropertyName("queryResult")]
        public List<SearchResultItem> QueryResult { get; set; }
    }

    public class SearchResultItem
    {
        [JsonPropertyName("attachment")]
        public bool Attachment { get; set; }

        [JsonPropertyName("excerpt")]
        public string Excerpt { get; set; }

        [JsonPropertyName("node")]
        public Document Node { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }
    }

    public record AddFileDto
    {
        public string Uuid { get; set; }
        public int Size { get; set; }
    }
}
