using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts
{
    public interface ICompanyServiceOfferingGetSingleHandler
    {
        Task<CompanyServiceOfferingGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
