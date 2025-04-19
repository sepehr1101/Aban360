using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using AutoMapper;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Implementations
{
    internal sealed class EquipmentBrokerCreateHandler : IEquipmentBrokerCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEquipmentBrokerCommandService _equipmentBrokerCommandService;
        public EquipmentBrokerCreateHandler(
            IMapper mapper,
            IEquipmentBrokerCommandService equipmentBrokerCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _equipmentBrokerCommandService = equipmentBrokerCommandService;
            _equipmentBrokerCommandService.NotNull(nameof(_equipmentBrokerCommandService));
        }

        public async Task Handle(EquipmentBrokerCreateDto createDto, CancellationToken cancellationToken)
        {
            var equipmentBroker = _mapper.Map<EquipmentBroker>(createDto);
            await _equipmentBrokerCommandService.Add(equipmentBroker);
        }
    }
}
