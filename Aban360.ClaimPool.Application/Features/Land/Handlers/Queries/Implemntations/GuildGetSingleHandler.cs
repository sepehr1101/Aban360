using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class GuildGetSingleHandler : IGuildGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IGuildQueryService _queryService;
        public GuildGetSingleHandler(
            IMapper mapper,
            IGuildQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<GuildGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            Guild guild = await _queryService.Get(id);
            return _mapper.Map<GuildGetDto>(guild);
        }
    }
}
