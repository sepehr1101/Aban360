using Aban360.BlobPool.Domain.Providers.Dto;

namespace Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts
{
    public interface IOpenKmQueryService
    {
        Task<string> GetDownloadLink(string uuid, bool oneTimeUse);
        Task<byte[]> GetFileBinary(string documentId);
        Task<FileListResponse> GetFilesByBillId(string billId);
        Task<FileListResponse> GetFilesInDirectory(string fieldId);
        Task<byte[]> GetImageThumbnail(string documentId);
        Task<SearchDocumentsResponse> SearchDocuments(string folderPath, string property, string path);
        Task<MetaDataProperties> GetMetaDataProperties(string documentId);
    }
}
