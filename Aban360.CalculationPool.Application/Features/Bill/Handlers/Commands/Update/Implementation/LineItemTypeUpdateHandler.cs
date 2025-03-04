using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    public class LineItemTypeUpdateHandler : ILineItemTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ILineItemTypeQueryService _lineItemTypeQueryService;
        public LineItemTypeUpdateHandler(
            IMapper mapper,
            ILineItemTypeQueryService lineItemTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _lineItemTypeQueryService = lineItemTypeQueryService;
            _lineItemTypeQueryService.NotNull(nameof(lineItemTypeQueryService));
        }

        public async Task Handle(LineItemTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var lineItemType = await _lineItemTypeQueryService.Get(updateDto.Id);
            if (lineItemType == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, lineItemType);
        }
    }
}
