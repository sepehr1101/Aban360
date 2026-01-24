using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface ISubscriptionAssignmentCommandService
    {
        Task Update(SubscriptionUpdateDto updateDto);
    }
}
