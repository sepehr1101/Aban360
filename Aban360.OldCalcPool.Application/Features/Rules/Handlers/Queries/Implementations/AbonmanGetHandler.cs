using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Implementations
{
    internal sealed class AbonmanGetHandler : IAbonmanGetHandler
    {
        private readonly IAbonmanQueryService _abonmanQueryService;
        public AbonmanGetHandler(IAbonmanQueryService abonmanQueryService)
        {
            _abonmanQueryService = abonmanQueryService;
            _abonmanQueryService.NotNull(nameof(abonmanQueryService));
        }

        public async Task<AbonmanGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            AbonmanGetDto result = await _abonmanQueryService.Get(id);
            return result;
        }
    }
}
