using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts
{
    public interface IOfferingGetSingleHandler
    {
        Task<OfferingGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
