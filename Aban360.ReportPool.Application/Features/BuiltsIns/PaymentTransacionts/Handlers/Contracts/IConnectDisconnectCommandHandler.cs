using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts
{
    public interface IConnectDisconnectCommandHandler
    {
        Task<ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto>> Handle(ConnectDisconnectPrintInputDto inputDto, IAppUser appUser, bool isConnect, CancellationToken cancellationToken);
        ICollection<NumericDictionary> GetCasues();
    }
}
