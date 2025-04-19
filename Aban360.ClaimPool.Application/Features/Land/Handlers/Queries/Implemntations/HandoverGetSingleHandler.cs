using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class HandoverGetSingleHandler : IHandoverGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IHandoverQueryService _handoverQueryService;
        public HandoverGetSingleHandler(
            IMapper mapper,
            IHandoverQueryService handoverQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _handoverQueryService = handoverQueryService;
            _handoverQueryService.NotNull(nameof(_handoverQueryService));
        }

        public async Task<HandoverGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var handover = await _handoverQueryService.Get(id);
            return _mapper.Map<HandoverGetDto>(handover);
        }
    }
}
