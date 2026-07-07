using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts
{
    public interface IConnectDisconnectMainWithCompanyHandler
    {
        Task<ReportOutput<ConnectDisconnectMainHeaderOutputDto, ConnectDisconnectMainByCompanyDataOutputDto>> Handle(ConnectDisconnectMainInputDto inputDto, CancellationToken cancellationToken);
    }
}