using Aban360.WorkflowPool.Domain.Features.Assignment.Dto.Commands;
using Aban360.WorkflowPool.Domain.Features.Assignment.Dto.Queries;
using Aban360.WorkflowPool.Domain.Features.Assignment.Entities;
using AutoMapper;

namespace Aban360.WorkflowPool.Application.Features.Assignment.Mappings
{
    public class WorkflowMapper : Profile
    {
        public WorkflowMapper()
        {
            CreateMap<WorkflowCreateDto, Workflow>();
            CreateMap<WorkflowDeleteDto, Workflow>();
            CreateMap<WorkflowUpdateDto, Workflow>();
            CreateMap<Workflow, WorkflowGetDto>();
        }
    }
}
