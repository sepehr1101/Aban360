using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;
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
            CreateMap<WorkflowJsonDefinitionUpdateDto, Workflow>();
            CreateMap<Workflow, WorkflowGetDto>();
            CreateMap<Workflow, WorkflowGetMasterDto>();
        }
    }
}
