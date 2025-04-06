using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Queries.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Aban360.WorkflowPool.Persistence.Features.Design.Queries.Contracts;
using AutoMapper;

namespace Aban360.WorkflowPool.Application.Features.Design.Handlers.Queries.Implementations
{
    internal sealed class WorkflowGetMasterHandler : IWorkflowGetMasterHandler
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowQueryService _workflowQueryService;
        public WorkflowGetMasterHandler(
            IMapper mapper,
            IWorkflowQueryService workflowQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _workflowQueryService = workflowQueryService;
            _workflowQueryService.NotNull(nameof(_workflowQueryService));
        }

        public async Task<ICollection<WorkflowGetMasterDto>> Handle(CancellationToken cancellationToken)
        {
            var workflow = await _workflowQueryService.GetMaster();
            return _mapper.Map<ICollection<WorkflowGetMasterDto>>(workflow);
        }
    }
}
