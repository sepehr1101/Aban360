using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts
{
    internal sealed class ZaribGetAllHandler : IZaribGetAllHandler
    {
        private readonly IZaribGetAllService _zaribGetAllService;
        public ZaribGetAllHandler(IZaribGetAllService zaribGetAllService)
        {
            _zaribGetAllService = zaribGetAllService;
            _zaribGetAllService.NotNull(nameof(zaribGetAllService));
        }

        public async Task<IEnumerable<ZaribGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<ZaribGetDto> result = await _zaribGetAllService.Get();
            return result;
        }
    }
}
