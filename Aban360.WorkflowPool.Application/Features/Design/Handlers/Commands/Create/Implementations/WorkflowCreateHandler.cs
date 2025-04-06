using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Commands.Create.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;
using Aban360.WorkflowPool.Persistence.Features.Design.Commands.Contracts;
using AutoMapper;

namespace Aban360.WorkflowPool.Application.Features.Design.Handlers.Commands.Create.Implementations
{
    internal sealed class WorkflowCreateHandler : IWorkflowCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowCommandService _workflowCommandService;
        public WorkflowCreateHandler(
            IMapper mapper,
            IWorkflowCommandService workflowCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _workflowCommandService = workflowCommandService;
            _workflowCommandService.NotNull(nameof(_workflowCommandService));
        }

        public async Task Handle(WorkflowCreateDto createDto, CancellationToken cancellationToken)
        {
            var workflow = _mapper.Map<Workflow>(createDto);
            workflow.ValidFrom = DateTime.Now;
            workflow.Name = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

            await _workflowCommandService.Add(workflow);
        }
    }
}
