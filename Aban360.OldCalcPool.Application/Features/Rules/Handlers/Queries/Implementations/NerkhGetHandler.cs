using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Implementations
{
    internal sealed class NerkhGetHandler : INerkhGetHandler
    {
        private readonly INerkhGetService _nerkhGetService;
        public NerkhGetHandler(INerkhGetService nerkhGetService)
        {
            _nerkhGetService = nerkhGetService;
            _nerkhGetService.NotNull(nameof(nerkhGetService));
        }

        public async Task<NerkhGetDto> Handle(int id, int nerkh, CancellationToken cancellationToken)
        {
            NerkhGetDto result = await _nerkhGetService.Get(id, nerkh);
            return result;
        }
    }
}
