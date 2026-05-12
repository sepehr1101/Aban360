using Aban360.ReportPool.Domain.Features.Dms;

namespace Aban360.ReportPool.Application.Features.Dms.Queries.Contracts
{
    public interface IClientDiscountGetAllHandler
    {
        Task<IEnumerable<ClientDiscount>> Handle(CancellationToken cancellationToken);
    }
}
