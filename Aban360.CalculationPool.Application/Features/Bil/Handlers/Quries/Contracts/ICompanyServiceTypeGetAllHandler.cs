using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts
{
    public interface ICompanyServiceTypeGetAllHandler
    {
        Task<ICollection<CompanyServiceTypeGetDto>> Handle(CancellationToken cancellationToken);
    }
}
