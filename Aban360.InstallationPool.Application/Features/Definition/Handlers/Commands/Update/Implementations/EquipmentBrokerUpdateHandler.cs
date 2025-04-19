using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using AutoMapper;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Implementations
{
    internal sealed class EquipmentBrokerUpdateHandler : IEquipmentBrokerUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEquipmentBrokerQueryService _equipmentBrokerQueryService;
        public EquipmentBrokerUpdateHandler(
            IMapper mapper,
            IEquipmentBrokerQueryService equipmentBrokerQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _equipmentBrokerQueryService = equipmentBrokerQueryService;
            _equipmentBrokerQueryService.NotNull(nameof(_equipmentBrokerQueryService));
        }

        public async Task Handle(EquipmentBrokerUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var equipmentBroker = await _equipmentBrokerQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, equipmentBroker);
        }
    }
}
