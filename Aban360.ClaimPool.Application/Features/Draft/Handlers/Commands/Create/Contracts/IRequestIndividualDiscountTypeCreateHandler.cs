using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts
{
    public interface IRequestIndividualDiscountTypeCreateHandler
    {
        Task Handle(RequestIndividualDiscountTypeCreateDto createDto, CancellationToken cancellationToken);
    }
}
