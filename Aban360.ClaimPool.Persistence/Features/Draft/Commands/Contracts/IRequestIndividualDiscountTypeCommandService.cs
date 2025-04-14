using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts
{
    public interface IRequestIndividualDiscountTypeCommandService
    {
        Task Add(RequestIndividualDiscountType requestIndividualDiscountType);
        Task Remove(RequestIndividualDiscountType requestIndividualDiscountType);
    }
}
