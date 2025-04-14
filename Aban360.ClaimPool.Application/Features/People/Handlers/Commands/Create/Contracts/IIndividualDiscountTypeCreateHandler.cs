using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts
{
    public interface IIndividualDiscountTypeCreateHandler
    {
        Task Handle(IndividualDiscountTypeCreateDto createDto, CancellationToken cancellationToken);
    }
}
