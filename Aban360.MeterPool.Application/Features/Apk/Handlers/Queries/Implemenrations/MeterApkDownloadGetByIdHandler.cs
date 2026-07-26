using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Apk.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Apk.Queries;
using Aban360.MeterPool.Persistence.Features.Apk.Queries.Contracts;

namespace Aban360.MeterPool.Application.Features.Apk.Handlers.Queries.Implemenrations
{
    internal sealed class MeterApkDownloadGetByIdHandler : IMeterApkDownloadGetByIdHandler
    {
        private readonly IMeterApkInfoQueryService _meterApkInfoQueryService;
        public MeterApkDownloadGetByIdHandler(IMeterApkInfoQueryService meterApkInfoQueryService)
        {
            _meterApkInfoQueryService = meterApkInfoQueryService;
            _meterApkInfoQueryService.NotNull(nameof(meterApkInfoQueryService));
        }
        public async Task<byte[]> Handle(short id, CancellationToken cancellationToken)
        {
            byte[] result = await _meterApkInfoQueryService.GetFile(id);
            return result;
        }
    }
}
