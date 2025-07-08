using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts
{
    public interface ICustomerInfoUpdateHandler
    {
        Task Handle(CustomerInfoLevel1UpdateDto updateDto, CancellationToken cancellationToken);
        Task Handle(CustomerInfoLevel2UpdateDto updateDto, CancellationToken cancellationToken);
    }
}
