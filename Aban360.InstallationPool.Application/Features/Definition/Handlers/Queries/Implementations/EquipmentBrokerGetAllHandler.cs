using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using AutoMapper;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Implementations
{
    internal sealed class EquipmentBrokerGetAllHandler : IEquipmentBrokerGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IEquipmentBrokerQueryService _equipmentBrokerQueryService;
        public EquipmentBrokerGetAllHandler(
            IMapper mapper,
            IEquipmentBrokerQueryService equipmentBrokerQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _equipmentBrokerQueryService = equipmentBrokerQueryService;
            _equipmentBrokerQueryService.NotNull(nameof(_equipmentBrokerQueryService));
        }

        public async Task<ICollection<EquipmentBrokerGetDto>> Handle(CancellationToken cancellationToken)
        {
            var equipmentBroker = await _equipmentBrokerQueryService.Get();
            return _mapper.Map<ICollection<EquipmentBrokerGetDto>>(equipmentBroker);
        }
    }
}
