using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Implementation
{
    public class LineItemTypeCreateHandler : ILineItemTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ILineItemTypeCommandService _lineItemTypeCommandService;
        public LineItemTypeCreateHandler(
            IMapper mapper,
            ILineItemTypeCommandService lineItemTypeCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _lineItemTypeCommandService = lineItemTypeCommandService;
            _lineItemTypeCommandService.NotNull(nameof(lineItemTypeCommandService));
        }

        public async Task Handle(LineItemTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var lineItemType = _mapper.Map<LineItemType>(createDto);
            if (lineItemType == null)
            {
                throw new InvalidDataException();
            }
            await _lineItemTypeCommandService.Add(lineItemType);
        }
    }
}
