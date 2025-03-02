using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    public class GuildGetSingleHandler : IGuildGetSingleHandler
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
            var guild = await _queryService.Get(id);
            if (guild == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<GuildGetDto>(guild);
        }
    }
}
