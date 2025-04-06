using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;
using AutoMapper;

namespace Aban360.WorkflowPool.Application.Features.Assignment.Mappings
{
    public class WorkflowStatusMapper : Profile
    {
        public WorkflowStatusMapper()
        {
            CreateMap<WorkflowStatus, WorkflowStatusGetDto>();
        }
    }
}
