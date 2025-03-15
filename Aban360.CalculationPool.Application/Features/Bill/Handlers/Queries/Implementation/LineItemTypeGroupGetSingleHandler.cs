using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class LineItemTypeGroupGetSingleHandler : ILineItemTypeGroupGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ILineItemTypeGroupQueryService _lineItemTypeGroupQueryService;
        public LineItemTypeGroupGetSingleHandler(
            IMapper mapper,
            ILineItemTypeGroupQueryService lineItemTypeGroupQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _lineItemTypeGroupQueryService = lineItemTypeGroupQueryService;
            _lineItemTypeGroupQueryService.NotNull(nameof(lineItemTypeGroupQueryService));
        }

        public async Task<LineItemTypeGroupGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            LineItemTypeGroup lineItemTypeGroup = await _lineItemTypeGroupQueryService.Get(id);
            return _mapper.Map<LineItemTypeGroupGetDto>(lineItemTypeGroup);
        }
    }
}
