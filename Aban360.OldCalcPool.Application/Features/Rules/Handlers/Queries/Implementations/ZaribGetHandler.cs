using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Implementations
{
    internal sealed class ZaribGetHandler : IZaribGetHandler
    {
        private readonly IZaribGetService _zaribGetService;
        public ZaribGetHandler(IZaribGetService zaribGetService)
        {
            _zaribGetService = zaribGetService;
            _zaribGetService.NotNull(nameof(zaribGetService));
        }

        public async Task<ZaribGetDto> Handle(int id,CancellationToken cancellationToken)
        {
            ZaribGetDto result = await _zaribGetService.Get(id);
            return result;
        }
    }
}
