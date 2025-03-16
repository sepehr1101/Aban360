using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    internal sealed class LineItemTypeGroupUpdateHandler : ILineItemTypeGroupUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ILineItemTypeGroupQueryService _lineItemTypeGroupQueryService;
        public LineItemTypeGroupUpdateHandler(
            IMapper mapper,
            ILineItemTypeGroupQueryService lineItemTypeGroupQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _lineItemTypeGroupQueryService = lineItemTypeGroupQueryService;
            _lineItemTypeGroupQueryService.NotNull(nameof(lineItemTypeGroupQueryService));
        }

        public async Task Handle(LineItemTypeGroupUpdateDto updateDto, CancellationToken cancellationToken)
        {
            LineItemTypeGroup lineItemTypeGroup = await _lineItemTypeGroupQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, lineItemTypeGroup);
        }
    }
}
