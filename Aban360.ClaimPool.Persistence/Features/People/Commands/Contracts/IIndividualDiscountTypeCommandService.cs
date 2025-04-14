using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts
{
    public interface IIndividualDiscountTypeCommandService
    {
        Task Add(IndividualDiscountType individualDiscountType);
        Task Remove(IndividualDiscountType individualDiscountType);
    }
}
