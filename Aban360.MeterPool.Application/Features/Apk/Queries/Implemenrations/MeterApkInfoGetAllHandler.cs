using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Apk.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Apk.Queries;
using Aban360.MeterPool.Persistence.Features.Apk.Queries.Contracts;

namespace Aban360.MeterPool.Application.Features.Apk.Queries.Implemenrations
{
    internal sealed class MeterApkInfoGetAllHandler : IMeterApkInfoGetAllHandler
    {
        private readonly IMeterApkInfoQueryService _meterApkInfoQueryService;
        public MeterApkInfoGetAllHandler(IMeterApkInfoQueryService meterApkInfoQueryService)
        {
            _meterApkInfoQueryService = meterApkInfoQueryService;
            _meterApkInfoQueryService.NotNull(nameof(meterApkInfoQueryService));
        }
        public async Task<IEnumerable<ApkInfoGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<ApkInfoGetDto> result = await _meterApkInfoQueryService.Get();
            return result;
        }
    }
}
