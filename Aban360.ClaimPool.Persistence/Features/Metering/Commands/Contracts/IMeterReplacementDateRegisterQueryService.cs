using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts
{
    public interface IMeterReplacementDateRegisterQueryService
    {
        Task Update(MeterReplacementDateRegisterUpdateDto input);
    }
}
