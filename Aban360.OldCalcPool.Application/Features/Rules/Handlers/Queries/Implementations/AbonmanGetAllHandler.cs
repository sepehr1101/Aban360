using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Implementations
{
    internal sealed class AbonmanGetAllHandler : IAbonmanGetAllHandler
    {
        private readonly IAbonmanQueryService _abonmanQueryService;
        public AbonmanGetAllHandler(IAbonmanQueryService abonmanQueryService)
        {
            _abonmanQueryService = abonmanQueryService;
            _abonmanQueryService.NotNull(nameof(abonmanQueryService));
        }

        public async Task<IEnumerable<AbonmanGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<AbonmanGetDto> result = await _abonmanQueryService.Get();
            return result;
        }
    }
}
