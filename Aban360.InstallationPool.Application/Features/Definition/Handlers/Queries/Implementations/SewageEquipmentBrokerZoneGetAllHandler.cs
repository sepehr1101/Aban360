using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using AutoMapper;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Implementations
{
    internal sealed class SewageEquipmentBrokerZoneGetAllHandler : ISewageEquipmentBrokerZoneGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ISewageEquipmentBrokerZoneQueryService _sewageEquipmentBrokerZoneQueryService;
        public SewageEquipmentBrokerZoneGetAllHandler(
            IMapper mapper,
            ISewageEquipmentBrokerZoneQueryService sewageEquipmentBrokerZoneQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _sewageEquipmentBrokerZoneQueryService = sewageEquipmentBrokerZoneQueryService;
            _sewageEquipmentBrokerZoneQueryService.NotNull(nameof(_sewageEquipmentBrokerZoneQueryService));
        }

        public async Task<ICollection<SewageEquipmentBrokerZoneGetDto>> Handle(CancellationToken cancellationToken)
        {
            var sewageEquipmentBrokerZone = await _sewageEquipmentBrokerZoneQueryService.Get();
            return _mapper.Map<ICollection<SewageEquipmentBrokerZoneGetDto>>(sewageEquipmentBrokerZone);
        }
    }
}
