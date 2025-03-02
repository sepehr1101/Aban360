using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    public class GuildUpdateHandler : IGuildUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IGuildQueryService _queryService;
        public GuildUpdateHandler(
            IMapper mapper,
            IGuildQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(GuildUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var guild = await _queryService.Get(updateDto.Id);
            if (guild == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, guild);
        }
    }
}
