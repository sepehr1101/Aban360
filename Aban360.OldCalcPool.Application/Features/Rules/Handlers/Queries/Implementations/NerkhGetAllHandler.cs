using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Implementations
{
    internal sealed class NerkhGetAllHandler : INerkhGetAllHandler
    {
        private readonly INerkhGetAllService _nerkhGetAllService;
        public NerkhGetAllHandler(INerkhGetAllService nerkhGetAllService)
        {
            _nerkhGetAllService = nerkhGetAllService;
            _nerkhGetAllService.NotNull(nameof(nerkhGetAllService));
        }

        public async Task<IEnumerable<NerkhGetDto>> Handle(int nerkh, CancellationToken cancellationToken)
        {
            IEnumerable<NerkhGetDto> result = await _nerkhGetAllService.Get(nerkh);
            return result;
        }
    }
}
