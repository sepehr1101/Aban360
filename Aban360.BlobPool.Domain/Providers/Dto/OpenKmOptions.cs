namespace Aban360.BlobPool.Domain.Providers.Dto
{
    public class OpenKmOptions
    {
        public const string SectionName = "OpenKm";
        public string BasePath { get; set; } = default!;
        public string BaseUrl { get; set; } = string.Empty;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string BaseDirectoryPath { get; set; } = default!;
        public string BaseDiscountPath { get; set; } = default!;
        public string TokenEndpoint { get; set; } = default!;
        public string GetFilesListEndpoint { get; set; } = default!;
        public string GetBinaryFileEndpoint { get; set; } = default!;
        public string SearchByMetadataEndpoint { get; set; } = default!;
        public string GetThumbnailEndpoint { get; set; } = default!;
        public string GetDownloadLinkEndpoint { get; set; } = default!;
        public string GeMetadataEndpoint { get; set; } = default!;
        public string PathExistsEndpoint { get; set; } = default!;
        public string AddFileEndpoint { get; set; } = default!;
        public string AddFolderEndpoint { get; set; } = default!;
        public string AddMetadataEndpoint { get; set; } = default!;
        public string MetadatableEndpoint { get; set; } = default!;
    }
}
