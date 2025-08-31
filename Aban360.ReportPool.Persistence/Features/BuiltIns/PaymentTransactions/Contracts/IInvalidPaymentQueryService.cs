using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts
{
    public interface IInvalidPaymentQueryService
    {
        Task<ReportOutput<InvalidPaymentHeaderOutputDto, InvalidPaymentDataOutputDto>> GetInfo(InvalidPaymentInputDto input);
    }
}
