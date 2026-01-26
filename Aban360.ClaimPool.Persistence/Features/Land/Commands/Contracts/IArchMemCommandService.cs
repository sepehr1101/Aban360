using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IArchMemCommandService
    {
        Task Insert(CustomerUpdateDto updateDto);
    }
}
