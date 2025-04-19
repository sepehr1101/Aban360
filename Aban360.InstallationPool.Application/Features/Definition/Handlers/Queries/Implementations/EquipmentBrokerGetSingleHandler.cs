using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using AutoMapper;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Implementations
{
    internal sealed class EquipmentBrokerGetSingleHandler : IEquipmentBrokerGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IEquipmentBrokerQueryService _equipmentBrokerQueryService;
        public EquipmentBrokerGetSingleHandler(
            IMapper mapper,
            IEquipmentBrokerQueryService equipmentBrokerQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _equipmentBrokerQueryService = equipmentBrokerQueryService;
            _equipmentBrokerQueryService.NotNull(nameof(_equipmentBrokerQueryService));
        }

        public async Task<EquipmentBrokerGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var equipmentBroker = await _equipmentBrokerQueryService.Get(id);
            return _mapper.Map<EquipmentBrokerGetDto>(equipmentBroker);
        }
    }
}
