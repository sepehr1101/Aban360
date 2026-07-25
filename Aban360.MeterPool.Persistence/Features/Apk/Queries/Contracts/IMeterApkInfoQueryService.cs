using Aban360.Common.BaseEntities;
using Aban360.MeterPool.Domain.Features.Apk.Queries;

namespace Aban360.MeterPool.Persistence.Features.Apk.Queries.Contracts
{
    public interface IMeterApkInfoQueryService
    {
        Task<IEnumerable<ApkInfoGetDto>> Get();
        Task<ApkInfoGetDto?> Get(short id);
        Task<ApkInfoGetDto?> Get(string version);
        Task<byte[]> GetFile(short id);
    }
}
