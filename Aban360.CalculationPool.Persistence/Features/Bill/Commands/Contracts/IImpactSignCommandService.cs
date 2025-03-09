using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface IImpactSignCommandService
    {
        Task Add(ImpactSign impactSign);
        void Remove(ImpactSign impactSign);
    }
}
