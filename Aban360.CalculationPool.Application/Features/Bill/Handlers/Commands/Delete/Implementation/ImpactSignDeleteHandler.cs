using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Implementation
{
    public class ImpactSignDeleteHandler : IImpactSignDeleteHandler
    {
        private readonly IImpactSignCommandService _impactSignCommandService;
        private readonly IImpactSignQueryService _impactSignQueryService;
        public ImpactSignDeleteHandler(
            IImpactSignCommandService impactSignCommandService,
            IImpactSignQueryService impactSignQueryService)
        {
            _impactSignCommandService = impactSignCommandService;
            _impactSignCommandService.NotNull(nameof(_impactSignCommandService));

            _impactSignQueryService = impactSignQueryService;
            _impactSignQueryService.NotNull(nameof(_impactSignQueryService));
        }

        public async Task Handle(ImpactSignDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var impactSign = await _impactSignQueryService.Get(deleteDto.Id);
            _impactSignCommandService.Remove(impactSign);
        }
    }
}
