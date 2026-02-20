using Aban360.BlobPool.Domain.Providers.Dto;
using System.Net.Http.Headers;

namespace Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts
{
    public interface IOpenKmQueryService
    {
        Task<string> GetDownloadLink(string uuid, bool oneTimeUse);
        Task<byte[]> GetFileBinary(string documentId);
        Task<FileListResponse> GetFilesDiscount(string id);
        Task<FileListResponse> GetFilesByBillId(string billId);
        Task<FileListResponse> GetFilesInDirectory(string fieldId);
        Task<byte[]> GetImageThumbnail(string documentId);
        Task<SearchDocumentsResponse> SearchDocuments(string folderPath, string property, string path);
        Task<MetaDataProperties> GetMetaDataProperties(string documentId);
        Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync();

        //commands
        Task<AddFileDto> AddFile(string path, StreamContent content);
        Task<string> AddFolderByBillId(string billId);
        Task<AddFileDto> AddFileByBillId(string billId, string localFilePath);
        Task AddOrUpdateMetadata(string body, string nodeId);
    }
}
