using System.Text.Json.Serialization;

namespace Aban360.BlobPool.Domain.Providers.Dto
{
    public sealed class FileListWithTitlesResponse
    {
        [JsonPropertyName("document")]
        public List<DocumentWithFileTitle> Documents { get; set; } = new();
    }

    public sealed class DocumentWithFileTitle : Document
    {
        [JsonPropertyName("fileTitle")]
        public string FileTitle { get; set; } = string.Empty;

        public DocumentWithFileTitle(Document source)
        {
            Author = source.Author;
            Created = source.Created;
            Path = source.Path;
            Permissions = source.Permissions;
            Subscribed = source.Subscribed;
            TenantId = source.TenantId;
            Title = source.Title;
            Uuid = source.Uuid;
            ActualVersion = source.ActualVersion;
            CheckedOut = source.CheckedOut;
            ConvertibleToPdf = source.ConvertibleToPdf;
            ConvertibleToSwf = source.ConvertibleToSwf;
            LastModified = source.LastModified;
            Locked = source.Locked;
            MimeType = source.MimeType;
            Signed = source.Signed;
        }
    }
}
