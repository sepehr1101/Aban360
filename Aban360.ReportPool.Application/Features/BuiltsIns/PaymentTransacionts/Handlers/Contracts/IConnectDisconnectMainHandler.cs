using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts
{
    public interface IConnectDisconnectMainHandler
    {
        Task<ReportOutput<ConnectDisconnectMainHeaderOutputDto, ConnectDisconnectMainDataOutputDto>> Handle(ConnectDisconnectMainInputDto inputDto, CancellationToken cancellationToken);
    }
}