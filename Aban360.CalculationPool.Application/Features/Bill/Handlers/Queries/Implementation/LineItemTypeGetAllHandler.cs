using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class LineItemTypeGetAllHandler : ILineItemTypeGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ILineItemTypeQueryService _lineItemTypeQueryService;
        public LineItemTypeGetAllHandler(
            IMapper mapper,
            ILineItemTypeQueryService lineItemTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _lineItemTypeQueryService = lineItemTypeQueryService;
            _lineItemTypeQueryService.NotNull(nameof(lineItemTypeQueryService));
        }

        public async Task<ICollection<LineItemTypeGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<LineItemType> lineItemType = await _lineItemTypeQueryService.Get();
            return _mapper.Map<ICollection<LineItemTypeGetDto>>(lineItemType);
        }
    }
}
