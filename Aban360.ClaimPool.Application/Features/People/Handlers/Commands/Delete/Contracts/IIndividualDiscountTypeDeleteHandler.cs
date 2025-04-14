using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts
{
    public interface IIndividualDiscountTypeDeleteHandler
    {
        Task Handle(IndividualDiscountTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
