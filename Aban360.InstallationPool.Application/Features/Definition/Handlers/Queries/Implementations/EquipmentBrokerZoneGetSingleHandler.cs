using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using AutoMapper;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Implementations
{
    internal sealed class EquipmentBrokerZoneGetSingleHandler : IEquipmentBrokerZoneGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IEquipmentBrokerZoneQueryService _equipmentBrokerZoneQueryService;
        public EquipmentBrokerZoneGetSingleHandler(
            IMapper mapper,
            IEquipmentBrokerZoneQueryService equipmentBrokerZoneQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _equipmentBrokerZoneQueryService = equipmentBrokerZoneQueryService;
            _equipmentBrokerZoneQueryService.NotNull(nameof(_equipmentBrokerZoneQueryService));
        }

        public async Task<EquipmentBrokerZoneGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var equipmentBrokerZone = await _equipmentBrokerZoneQueryService.Get(id);
            return _mapper.Map<EquipmentBrokerZoneGetDto>(equipmentBrokerZone);
        }
    }
}
