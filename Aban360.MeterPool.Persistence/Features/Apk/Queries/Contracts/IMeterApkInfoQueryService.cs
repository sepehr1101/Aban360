using Aban360.MeterPool.Domain.Features.Apk.Queries;

namespace Aban360.MeterPool.Persistence.Features.Apk.Queries.Contracts
{
    public interface IMeterApkInfoQueryService
    {
        Task<IEnumerable<ApkInfoGetDto>> Get();
        Task<ApkInfoGetDto> GetLatest();
        Task<string> GetLatestVersion();
        Task<ApkInfoGetDto?> Get(short id);
        Task<ApkInfo?> Get(string version);
        Task<byte[]> GetFile(short id);
    }
}
