using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.MeterPool.Application.Features.Apk.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Apk.Queries;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Apk.Queries.Contracts;

namespace Aban360.MeterPool.Application.Features.Apk.Queries.Implemenrations
{
    internal sealed class MeterApkInfoValidationHandler : IMeterApkInfoValidationHandler
    {
        private readonly IMeterApkInfoQueryService _meterApkInfoQueryService;
        public MeterApkInfoValidationHandler(IMeterApkInfoQueryService meterApkInfoQueryService)
        {
            _meterApkInfoQueryService = meterApkInfoQueryService;
            _meterApkInfoQueryService.NotNull(nameof(meterApkInfoQueryService));
        }
        public async Task<MeterApkValidateOutputDto> Handle(string version, CancellationToken cancellationToken)
        {
            ApkInfoGetDto? apkInfo = await _meterApkInfoQueryService.Get(version);
            IEnumerable<ApkInfoGetDto> apkList = await _meterApkInfoQueryService.Get();
            string latestValidVersion = apkList?.Where(a => a.RemovedBy is null && a.ExpiredBy is null)?.FirstOrDefault()?.Version ?? string.Empty;
            if (apkInfo is null)
            {
                throw new ReadingException(ExceptionLiterals.NotFoundMeterApkFileVersion);
            }
            if (apkInfo.RemovedBy is not null || apkInfo.ExpiredBy is not null)
            {
                return new MeterApkValidateOutputDto(false, latestValidVersion, version);
            }
            return new MeterApkValidateOutputDto(true, latestValidVersion, version);
        }
    }
}
