using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Design.Handlers.Queries.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Aban360.WorkflowPool.Persistence.Features.Design.Queries.Contracts;
using AutoMapper;

namespace Aban360.WorkflowPool.Application.Features.Design.Handlers.Queries.Implementations
{
    internal sealed class StateGetAllHandler : IStateGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IStateQueryService _stateQueryService;
        public StateGetAllHandler(
            IMapper mapper,
            IStateQueryService stateQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _stateQueryService = stateQueryService;
            _stateQueryService.NotNull(nameof(_stateQueryService));
        }

        public async Task<ICollection<StateGetDto>> Handle(CancellationToken cancellationToken)
        {
            var state = await _stateQueryService.Get();
            return _mapper.Map<ICollection<StateGetDto>>(state);
        }
    }
}
