using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using AutoMapper;

namespace Aban360.InstallationPool.Application.Features.Definition.Mappings
{
    public class EquipmentBrokerMapper : Profile
    {
        public EquipmentBrokerMapper()
        {
            CreateMap<EquipmentBrokerCreateDto, EquipmentBroker>();
            CreateMap<EquipmentBrokerDeleteDto, EquipmentBroker>();
            CreateMap<EquipmentBrokerUpdateDto, EquipmentBroker>();
            CreateMap<EquipmentBroker, EquipmentBrokerGetDto>();
        }
    }
}