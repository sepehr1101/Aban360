using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Implementation
{
    internal sealed class LineItemTypeDeleteHandler : ILineItemTypeDeleteHandler
    {
        private readonly ILineItemTypeCommandService _lineItemTypeCommandService;
        private readonly ILineItemTypeQueryService _lineItemTypeQueryService;
        public LineItemTypeDeleteHandler(
            ILineItemTypeCommandService lineItemTypeCommandService,
            ILineItemTypeQueryService lineItemTypeQueryService)
        {
            _lineItemTypeCommandService = lineItemTypeCommandService;
            _lineItemTypeCommandService.NotNull(nameof(lineItemTypeCommandService));

            _lineItemTypeQueryService = lineItemTypeQueryService;
            _lineItemTypeQueryService.NotNull(nameof(lineItemTypeQueryService));
        }

        public async Task Handle(LineItemTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            LineItemType lineItemType = await _lineItemTypeQueryService.Get(deleteDto.Id);
            await _lineItemTypeCommandService.Remove(lineItemType);
        }
    }
}
