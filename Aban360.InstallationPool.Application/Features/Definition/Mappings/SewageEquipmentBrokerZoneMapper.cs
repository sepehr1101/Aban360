using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using AutoMapper;

namespace Aban360.InstallationPool.Application.Features.Definition.Mappings
{
    public class SewageEquipmentBrokerZoneMapper : Profile
    {
        public SewageEquipmentBrokerZoneMapper()
        {
            CreateMap<SewageEquipmentBrokerZoneCreateDto, SewageEquipmentBrokerZone>();
            CreateMap<SewageEquipmentBrokerZoneDeleteDto, SewageEquipmentBrokerZone>();
            CreateMap<SewageEquipmentBrokerZoneUpdateDto, SewageEquipmentBrokerZone>();
            CreateMap<SewageEquipmentBrokerZone, SewageEquipmentBrokerZoneGetDto>();
        }
    }
}
