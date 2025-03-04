using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts
{
    public interface ICompanyServiceGetAllHandler
    {
        Task<ICollection<CompanyServiceGetDto>> Handle(CancellationToken cancellationToken);
    }
}
