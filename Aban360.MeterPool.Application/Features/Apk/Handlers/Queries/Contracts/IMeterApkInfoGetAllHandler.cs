using Aban360.MeterPool.Domain.Features.Apk.Queries;

namespace Aban360.MeterPool.Application.Features.Apk.Handlers.Queries.Contracts
{
    public interface IMeterApkInfoGetAllHandler
    {
        Task<IEnumerable<ApkInfoGetDto>> Handle(CancellationToken cancellationToken);
    }
}
