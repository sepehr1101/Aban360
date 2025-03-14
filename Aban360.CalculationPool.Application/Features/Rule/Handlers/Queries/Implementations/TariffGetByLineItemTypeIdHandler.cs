﻿using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations
{
    public class TariffGetByLineItemTypeIdHandler : ITariffGetByLineItemTypeIdHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffQueryService _tariffQueryService;
        public TariffGetByLineItemTypeIdHandler(
            IMapper mapper,
            ITariffQueryService tariffQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffQueryService = tariffQueryService;
            _tariffQueryService.NotNull(nameof(tariffQueryService));
        }

        public async Task<ICollection<TariffGetDto>> Handle(short id, CancellationToken cancellationToken)
        {
            var tariff = await _tariffQueryService.GetByLineItemType(id);
            if (tariff == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<TariffGetDto>>(tariff);
        }
    }
}
