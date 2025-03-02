using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts
{
    public interface IOfferingGetAllHandler
    {
        Task<ICollection<OfferingGetDto>> Handle(CancellationToken cancellationToken);
    }
}
