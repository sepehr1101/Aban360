using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts
{
    public interface ISetInstallmentRequestHandler
    {
        Task<ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto>> Handle(InstallmentRequestInputDto inputDto, CancellationToken cancellationToken);
    }
}
