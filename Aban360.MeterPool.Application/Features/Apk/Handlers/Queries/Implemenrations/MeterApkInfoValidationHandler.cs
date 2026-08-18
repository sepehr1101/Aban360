using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Apk.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Apk.Queries;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Apk.Queries.Contracts;

namespace Aban360.MeterPool.Application.Features.Apk.Handlers.Queries.Implemenrations
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
            ApkInfo? apkInfo = await _meterApkInfoQueryService.GetValid(version);
            string latestValidVersion = (await _meterApkInfoQueryService.GetLatestVersion())?.Version ?? string.Empty;

            //if (apkInfo is null)
            //{
            ////throw new ReadingException(ExceptionLiterals.NotFoundMeterApkFileVersion);
            //}
            if (apkInfo is null || !apkInfo.IsActive)
            {
                return new MeterApkValidateOutputDto(false, latestValidVersion, version);
            }
            return new MeterApkValidateOutputDto(true, latestValidVersion, version);
        }
    }
}
