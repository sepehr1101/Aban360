using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts
{
    public interface IServiceLinkPaymentDetailHandler
    {
        Task<ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>> Handle(PaymentDetailInputDto input, CancellationToken cancellationToken);
    }
}
