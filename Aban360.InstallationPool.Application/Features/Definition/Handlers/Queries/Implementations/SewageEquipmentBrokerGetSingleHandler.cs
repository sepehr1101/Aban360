using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using AutoMapper;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Implementations
{
    internal sealed class SewageEquipmentBrokerGetSingleHandler : ISewageEquipmentBrokerGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ISewageEquipmentBrokerQueryService _sewageEquipmentBrokerQueryService;
        public SewageEquipmentBrokerGetSingleHandler(
            IMapper mapper,
            ISewageEquipmentBrokerQueryService sewageEquipmentBrokerQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _sewageEquipmentBrokerQueryService = sewageEquipmentBrokerQueryService;
            _sewageEquipmentBrokerQueryService.NotNull(nameof(_sewageEquipmentBrokerQueryService));
        }

        public async Task<SewageEquipmentBrokerGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var sewageEquipmentBroker = await _sewageEquipmentBrokerQueryService.Get(id);
            return _mapper.Map<SewageEquipmentBrokerGetDto>(sewageEquipmentBroker);
        }
    }
}
