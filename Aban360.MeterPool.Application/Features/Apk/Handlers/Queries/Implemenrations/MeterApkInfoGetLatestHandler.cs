using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Apk.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Apk.Queries;
using Aban360.MeterPool.Persistence.Features.Apk.Queries.Contracts;

namespace Aban360.MeterPool.Application.Features.Apk.Handlers.Queries.Implemenrations
{
    internal sealed class MeterApkInfoGetLatestHandler : IMeterApkInfoGetLatestHandler
    {
        private readonly IMeterApkInfoQueryService _meterApkInfoQueryService;
        public MeterApkInfoGetLatestHandler(IMeterApkInfoQueryService meterApkInfoQueryService)
        {
            _meterApkInfoQueryService = meterApkInfoQueryService;
            _meterApkInfoQueryService.NotNull(nameof(meterApkInfoQueryService));
        }
        public async Task<ApkInfoGetDto> Handle( CancellationToken cancellationToken)
        {
            ApkInfoGetDto result = await _meterApkInfoQueryService.GetLatest();
            return result;
        }
    }
}
