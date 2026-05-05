using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts
{
    public interface IConnectDisconnectPrintHandler
    {
        Task<ConnectDisconnectPrintOutputDto> Handle(ConnectDisconnectPrintInputDto inputDto, CancellationToken cancellationToken);
    }
}
