using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Design.Handlers.Queries.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Aban360.WorkflowPool.Persistence.Features.Design.Queries.Contracts;
using AutoMapper;

namespace Aban360.WorkflowPool.Application.Features.Design.Handlers.Queries.Implementations
{
    internal sealed class StateGetSingleHandler : IStateGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IStateQueryService _stateQueryService;
        public StateGetSingleHandler(
            IMapper mapper,
            IStateQueryService stateQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _stateQueryService = stateQueryService;
            _stateQueryService.NotNull(nameof(_stateQueryService));
        }

        public async Task<StateGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            var state = await _stateQueryService.Get(id);
            return _mapper.Map<StateGetDto>(state);
        }
    }
}
