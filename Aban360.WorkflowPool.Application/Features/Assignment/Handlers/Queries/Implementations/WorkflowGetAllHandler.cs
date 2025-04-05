using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Queries.Contracts;
using Aban360.WorkflowPool.Domain.Features.Assignment.Dto.Queries;
using Aban360.WorkflowPool.Persistence.Features.Assignment.Queries.Contracts;
using AutoMapper;

namespace Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Queries.Implementations
{
    internal sealed class WorkflowGetAllHandler : IWorkflowGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowQueryService _workflowQueryService;
        public WorkflowGetAllHandler(
            IMapper mapper,
            IWorkflowQueryService workflowQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _workflowQueryService = workflowQueryService;
            _workflowQueryService.NotNull(nameof(_workflowQueryService));
        }

        public async Task<ICollection<WorkflowGetDto>> Handle(CancellationToken cancellationToken)
        {
            var workflow = await _workflowQueryService.Get();
            return _mapper.Map<ICollection<WorkflowGetDto>>(workflow);
        }
    }
}
