using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;

namespace Aban360.MeterPool.Application.Features.Apk.Handlers.Queries.Contracts
{
    public interface IMeterApkInfoValidationHandler
    {
        Task<MeterApkValidateOutputDto> Handle(string version, CancellationToken cancellationToken);
    }
}
