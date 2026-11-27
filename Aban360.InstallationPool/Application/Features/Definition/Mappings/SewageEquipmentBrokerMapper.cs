using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using AutoMapper;

namespace Aban360.InstallationPool.Application.Features.Definition.Mappings
{
    public class SewageEquipmentBrokerMapper : Profile
    {
        public SewageEquipmentBrokerMapper()
        {
            CreateMap<SewageEquipmentBrokerCreateDto, SewageEquipmentBroker>();
            CreateMap<SewageEquipmentBrokerDeleteDto, SewageEquipmentBroker>();
            CreateMap<SewageEquipmentBrokerUpdateDto, SewageEquipmentBroker>();
            CreateMap<SewageEquipmentBroker, SewageEquipmentBrokerGetDto>();
        }
    }
}