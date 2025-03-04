using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Implementation
{
    public class LineItemTypeGroupGetAllHandler : ILineItemTypeGroupGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ILineItemTypeGroupQueryService _lineItemTypeGroupQueryService;
        public LineItemTypeGroupGetAllHandler(
            IMapper mapper,
            ILineItemTypeGroupQueryService lineItemTypeGroupQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _lineItemTypeGroupQueryService = lineItemTypeGroupQueryService;
            _lineItemTypeGroupQueryService.NotNull(nameof(lineItemTypeGroupQueryService));
        }

        public async Task<ICollection<LineItemTypeGroupGetDto>> Handle(CancellationToken cancellationToken)
        {
            var lineItemTypeGroup = await _lineItemTypeGroupQueryService.Get();
            if (lineItemTypeGroup == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<LineItemTypeGroupGetDto>>(lineItemTypeGroup);
        }
    }
}
