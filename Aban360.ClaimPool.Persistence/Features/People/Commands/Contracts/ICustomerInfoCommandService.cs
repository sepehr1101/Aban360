using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts
{
    public interface ICustomerInfoCommandService
    {
        Task Update(CustomerInfoUpdateDto updateDto);
    }
}
