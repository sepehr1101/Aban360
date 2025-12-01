using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Implementations
{
    internal sealed class SGetHandler : ISGetHandler
    {
        private readonly ISQueryService _sQueryService;
        public SGetHandler(ISQueryService sQueryService)
        {
            _sQueryService = sQueryService;
            _sQueryService.NotNull(nameof(sQueryService));
        }

        public async Task<IEnumerable<SGetDto>> Handle(string fromDateJalali, string toDateJalali, CancellationToken cancellationToken)
        {
            IEnumerable<SGetDto> result = await _sQueryService.Get(fromDateJalali, toDateJalali);
            return result;
        }
    }
}
