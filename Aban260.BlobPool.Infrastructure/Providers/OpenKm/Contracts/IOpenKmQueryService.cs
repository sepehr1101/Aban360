using Aban360.BlobPool.Domain.Providers.Dto;

namespace Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts
{
    public interface IOpenKmQueryService
    {
        Task<FileListResponse> GetFilesByBillId(string billId);
    }
}
