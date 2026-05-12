using Aban360.ReportPool.Domain.Features.Dms;

namespace Aban360.ReportPool.Application.Features.Dms.Commands.Contracts
{
    public interface IClientDiscountUpdateHandler
    {
        Task Handle(ClientDiscountUpdateDto input, CancellationToken cancellationToken);
    }
}
