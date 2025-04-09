using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Design.Handlers.Commands.Update.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Aban360.WorkflowPool.Persistence.Features.Design.Queries.Contracts;
using AutoMapper;

namespace Aban360.WorkflowPool.Application.Features.Design.Handlers.Commands.Update.Implementations
{
    internal sealed class StateUpdateHandler : IStateUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IStateQueryService _stateQueryService;
        public StateUpdateHandler(
            IMapper mapper,
            IStateQueryService stateQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _stateQueryService = stateQueryService;
            _stateQueryService.NotNull(nameof(_stateQueryService));
        }

        public async Task Handle(StateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var state = await _stateQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, state);
            state.ValidFrom = DateTime.Now;
            state.Hash = "hash";
            state.InsertLogInfo = "insertLogInfo";

        }
    }
}
