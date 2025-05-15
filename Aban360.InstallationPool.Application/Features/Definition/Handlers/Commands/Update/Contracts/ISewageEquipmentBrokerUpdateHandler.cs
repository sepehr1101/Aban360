using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Contracts
{
    public interface ISewageEquipmentBrokerUpdateHandler
    {
        Task Handle(SewageEquipmentBrokerUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
