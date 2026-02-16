using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.Usp.Input;
using Aban360.ReportPool.Domain.Features.Usp.Output;

namespace Aban360.ReportPool.Application.Features.Usp.Handlers.Contracts
{
    public interface IUspPayment2Handler
    {
        Task<ReportOutput<UspPayment2Header, UspPayment2Output>> Handle(UspPayment2Input input, CancellationToken cancellationToken);
    }
}