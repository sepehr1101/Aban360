﻿using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class OfferingGroupGetSingleHandler : IOfferingGroupGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfferingGroupQueryService _offeringGroupQueryService;
        public OfferingGroupGetSingleHandler(
            IMapper mapper,
            IOfferingGroupQueryService offeringGroupQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _offeringGroupQueryService = offeringGroupQueryService;
            _offeringGroupQueryService.NotNull(nameof(offeringGroupQueryService));
        }

        public async Task<OfferingGroupGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var offeringGroup = await _offeringGroupQueryService.Get(id);
            if (offeringGroup == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<OfferingGroupGetDto>(offeringGroup);
        }
    }
}
