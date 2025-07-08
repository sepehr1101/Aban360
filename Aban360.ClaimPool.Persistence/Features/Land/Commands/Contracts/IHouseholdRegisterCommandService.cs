using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IHouseholdRegisterCommandService
    {
        Task Update(HouseholdRegisterUpdateDto updateDto, string date);
    }
}

