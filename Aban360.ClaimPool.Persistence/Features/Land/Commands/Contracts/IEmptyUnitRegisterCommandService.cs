using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IEmptyUnitRegisterCommandService
    {
        Task Update(EmptyUnitRegisterUpdateDto updateDto, string date);
    }
}
