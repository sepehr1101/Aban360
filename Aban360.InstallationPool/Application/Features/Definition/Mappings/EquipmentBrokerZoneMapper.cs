using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using AutoMapper;

namespace Aban360.InstallationPool.Application.Features.Definition.Mappings
{
    public class EquipmentBrokerZoneMapper : Profile
    {
        public EquipmentBrokerZoneMapper()
        {
            CreateMap<EquipmentBrokerZoneCreateDto, EquipmentBrokerZone>();
            CreateMap<EquipmentBrokerZoneDeleteDto, EquipmentBrokerZone>();
            CreateMap<EquipmentBrokerZoneUpdateDto, EquipmentBrokerZone>();
            CreateMap<EquipmentBrokerZone, EquipmentBrokerZoneGetDto>();
        }
    }
}
