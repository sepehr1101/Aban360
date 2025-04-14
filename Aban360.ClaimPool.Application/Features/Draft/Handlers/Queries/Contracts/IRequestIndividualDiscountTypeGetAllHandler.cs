using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Contracts
{
    public interface IRequestIndividualDiscountTypeGetAllHandler
    {
        Task<ICollection<RequestIndividualDiscountTypeGetDto>> Handle(CancellationToken cancellationToken);
    }
}
