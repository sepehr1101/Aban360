using Aban360.MeterPool.Domain.Features.Apk.Queries;

namespace Aban360.MeterPool.Persistence.Features.Apk.Queries.Contracts
{
    public interface IMeterApkInfoQueryService
    {
        Task<IEnumerable<ApkInfoGetDto>> GetValid();
        Task<ApkInfoGetDto?> GetLatestVersion();
        Task<ApkInfoGetDto?> GetValid(short id);
        Task<ApkInfo?> GetValid(string version);
        Task<ApkInfo?> GetLatestValidVersion();
        Task<byte[]> GetFile(short id);
    }
}
