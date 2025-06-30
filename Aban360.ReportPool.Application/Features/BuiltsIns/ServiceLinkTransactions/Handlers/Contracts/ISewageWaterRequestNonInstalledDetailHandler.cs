using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface ISewageWaterRequestNonInstalledDetailHandler
    {
        Task<ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledDetailDataOutputDto>> Handle(SewageWaterRequestNonInstalledInputDto input, CancellationToken cancellationToken);
    }
}