using Aban360.ReportPool.Domain.Features.Dms;

namespace Aban360.ReportPool.Application.Features.Dms.Commands.Contracts
{
    public interface IClientDiscountInsertHandler
    {
        Task Handle(ClientDiscountInsertDto input, CancellationToken cancellationToken);
    }
}
