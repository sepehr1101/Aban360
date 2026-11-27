using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;
using AutoMapper;

namespace Aban360.WorkflowPool.Application.Features.Design.Mappings
{
    public class StateMapper : Profile
    {
        public StateMapper()
        {
            CreateMap<StateCreateDto, State>();
            CreateMap<StateDeleteDto, State>();
            CreateMap<StateUpdateDto, State>();
            CreateMap<State, StateGetDto>();
        }
    }
}
