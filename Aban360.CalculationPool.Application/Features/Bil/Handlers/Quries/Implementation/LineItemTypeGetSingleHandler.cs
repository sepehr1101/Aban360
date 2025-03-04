﻿using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Implementation
{
    public class LineItemTypeGetSingleHandler : ILineItemTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ILineItemTypeQueryService _lineItemTypeQueryService;
        public LineItemTypeGetSingleHandler(
            IMapper mapper,
            ILineItemTypeQueryService lineItemTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _lineItemTypeQueryService = lineItemTypeQueryService;
            _lineItemTypeQueryService.NotNull(nameof(lineItemTypeQueryService));
        }

        public async Task<LineItemTypeGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var lineItemType = await _lineItemTypeQueryService.Get(id);
            if (lineItemType == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<LineItemTypeGetDto>(lineItemType);
        }
    }
}
