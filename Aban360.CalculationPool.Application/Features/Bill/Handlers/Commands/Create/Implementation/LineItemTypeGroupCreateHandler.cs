using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class LineItemTypeGroupCreateHandler : ILineItemTypeGroupCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ILineItemTypeGroupCommandService _lineItemTypeGroupCommandService;
        public LineItemTypeGroupCreateHandler(
            IMapper mapper,
            ILineItemTypeGroupCommandService lineItemTypeGroupCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _lineItemTypeGroupCommandService = lineItemTypeGroupCommandService;
            _lineItemTypeGroupCommandService.NotNull(nameof(lineItemTypeGroupCommandService));
        }

        public async Task Handle(LineItemTypeGroupCreateDto createDto, CancellationToken cancellationToken)
        {
            var lineItemTypeGroup = _mapper.Map<LineItemTypeGroup>(createDto);
            if (lineItemTypeGroup == null)
            {
                throw new InvalidDataException();
            }
            await _lineItemTypeGroupCommandService.Add(lineItemTypeGroup);
        }
    }
}
