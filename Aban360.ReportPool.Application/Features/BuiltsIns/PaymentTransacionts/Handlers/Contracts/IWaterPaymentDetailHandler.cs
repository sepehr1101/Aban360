using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts
{
    public interface IWaterPaymentDetailHandler
    {
        Task<ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>> Handle(PaymentDetailInputDto input, CancellationToken cancellationToken);
    }
}
