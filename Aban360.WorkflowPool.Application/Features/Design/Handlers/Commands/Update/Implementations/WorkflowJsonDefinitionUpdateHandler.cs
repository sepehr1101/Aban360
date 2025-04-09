using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Commands.Update.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Aban360.WorkflowPool.Persistence.Features.Design.Queries.Contracts;
using AutoMapper;

namespace Aban360.WorkflowPool.Application.Features.Design.Handlers.Commands.Update.Implementations
{
    internal sealed class WorkflowJsonDefinitionUpdateHandler : IWorkflowJsonDefinitionUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowQueryService _workflowQueryService;
        public WorkflowJsonDefinitionUpdateHandler(
            IMapper mapper,
            IWorkflowQueryService workflowQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _workflowQueryService = workflowQueryService;
            _workflowQueryService.NotNull(nameof(_workflowQueryService));
        }

        public async Task Handle(WorkflowJsonDefinitionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var workflow = await _workflowQueryService.Get(updateDto.Id);
            workflow.JsonDefinition = updateDto.JsonDefinition;

            _mapper.Map(updateDto, workflow);
        }
    }
}
