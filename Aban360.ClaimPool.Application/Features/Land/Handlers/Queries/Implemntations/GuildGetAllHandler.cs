using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    public class GuildGetAllHandler : IGuildGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IGuildQueryService _queryService;
        public GuildGetAllHandler(
            IMapper mapper,
            IGuildQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ICollection<GuildGetDto>> Handle(CancellationToken cancellationToken)
        {
            var guild = await _queryService.Get();
            if (guild == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<GuildGetDto>>(guild);
        }
    }
}
