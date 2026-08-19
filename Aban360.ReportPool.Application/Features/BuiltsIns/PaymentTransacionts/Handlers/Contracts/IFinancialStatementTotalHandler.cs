using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts
{
    public interface IFinancialStatementTotalHandler
    {
        Task<ReportOutput<FinancialStatementHeaderOutputDto, FinancialStatementDataOutputDto>> Handle(FinancialStatementInputDto input, CancellationToken cancellationToken);
    }
}
