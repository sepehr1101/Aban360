using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    public interface IGuildQueryHandler
    {
        Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken);
    }

    internal sealed class GuildQueryHandler : IGuildQueryHandler
    {
        private readonly IGuildQueryService _guildService;
        public GuildQueryHandler(IGuildQueryService guildQueryService)
        {
            _guildService = guildQueryService;
            _guildService.NotNull(nameof(guildQueryService));
        }

        public async Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken)
        {
            return await _guildService.Get();
        }
    }
}
