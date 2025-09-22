using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Implementations
{
    internal sealed class ZaribCGetAllHandler : IZaribCGetAllHandler
    {
        private readonly IZaribCQueryService _zaribCQueryService;
        public ZaribCGetAllHandler(IZaribCQueryService zaribCQueryService)
        {
            _zaribCQueryService = zaribCQueryService;
            _zaribCQueryService.NotNull(nameof(zaribCQueryService));
        }

        public async Task<IEnumerable<ZaribCQueryDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<ZaribCQueryDto> result = await _zaribCQueryService.GetZaribC();
            return result;
        }
    }
}
