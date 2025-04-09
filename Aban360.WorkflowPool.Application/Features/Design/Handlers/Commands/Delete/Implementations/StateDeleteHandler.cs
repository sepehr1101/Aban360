using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Design.Handlers.Commands.Delete.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Aban360.WorkflowPool.Persistence.Features.Design.Commands.Contracts;
using Aban360.WorkflowPool.Persistence.Features.Design.Queries.Contracts;

namespace Aban360.WorkflowPool.Application.Features.Design.Handlers.Commands.Delete.Implementations
{
    internal sealed class StateDeleteHandler : IStateDeleteHandler
    {
        private readonly IStateCommandService _stateCommandService;
        private readonly IStateQueryService _stateQueryService;
        public StateDeleteHandler(
            IStateCommandService stateCommandService,
            IStateQueryService stateQueryService)
        {
            _stateCommandService = stateCommandService;
            _stateCommandService.NotNull(nameof(_stateCommandService));

            _stateQueryService = stateQueryService;
            _stateQueryService.NotNull(nameof(_stateQueryService));
        }

        public async Task Handle(StateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var state = await _stateQueryService.Get(deleteDto.Id);
            await _stateCommandService.Remove(state);
        }
    }
}
