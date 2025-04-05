using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Commands.Update.Contracts;
using Aban360.WorkflowPool.Domain.Features.Assignment.Dto.Commands;
using Aban360.WorkflowPool.Persistence.Features.Assignment.Queries.Contracts;
using AutoMapper;

namespace Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Commands.Update.Implementations
{
    internal sealed class WorkflowUpdateHandler : IWorkflowUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowQueryService _workflowQueryService;
        public WorkflowUpdateHandler(
            IMapper mapper,
            IWorkflowQueryService workflowQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _workflowQueryService = workflowQueryService;
            _workflowQueryService.NotNull(nameof(_workflowQueryService));
        }

        public async Task Handle(WorkflowUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var workflow = await _workflowQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, workflow);
        }
    }
}
