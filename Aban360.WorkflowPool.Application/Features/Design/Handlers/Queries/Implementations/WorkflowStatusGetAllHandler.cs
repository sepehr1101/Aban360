using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Queries.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Aban360.WorkflowPool.Persistence.Features.Design.Queries.Contracts;
using AutoMapper;

namespace Aban360.WorkflowStatusPool.Application.Features.Design.Handlers.Queries.Implementations
{
    internal sealed class WorkflowStatusGetAllHandler : IWorkflowStatusGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowStatusQueryService _workflowStatusQueryService;
        public WorkflowStatusGetAllHandler(
            IMapper mapper,
            IWorkflowStatusQueryService workflowStatusQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _workflowStatusQueryService = workflowStatusQueryService;
            _workflowStatusQueryService.NotNull(nameof(_workflowStatusQueryService));
        }

        public async Task<ICollection<WorkflowStatusGetDto>> Handle(CancellationToken cancellationToken)
        {
            var workflowStatus = await _workflowStatusQueryService.Get();
            return _mapper.Map<ICollection<WorkflowStatusGetDto>>(workflowStatus);
        }
    }
}
