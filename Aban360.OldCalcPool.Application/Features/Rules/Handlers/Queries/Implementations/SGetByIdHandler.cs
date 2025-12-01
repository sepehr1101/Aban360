using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Implementations
{
    internal sealed class SGetByIdHandler : ISGetByIdHandler
    {
        private readonly ISQueryService _sQueryService;
        public SGetByIdHandler(ISQueryService sQueryService)
        {
            _sQueryService = sQueryService;
            _sQueryService.NotNull(nameof(sQueryService));
        }

        public async Task<SGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            SGetDto result = await _sQueryService.Get(id);
            return result;
        }
    }
}
