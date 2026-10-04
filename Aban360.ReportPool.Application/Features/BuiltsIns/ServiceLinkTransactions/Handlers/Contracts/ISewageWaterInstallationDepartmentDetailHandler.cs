using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface ISewageWaterInstallationDepartmentDetailHandler
    {
        Task<ReportOutput<SewageWaterInstallationDetailHeaderOutputDto, SewageWaterInstallationDetailDataOutputDto>> Handle(SewageWaterInstallationInputDto input, CancellationToken cancellationToken);
    }
}
