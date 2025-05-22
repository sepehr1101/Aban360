using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts
{
    public interface IRequestWaterMeterUpdateHandler
    {
        Task Handle(IAppUser currentUser, WaterMeterRequestUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
