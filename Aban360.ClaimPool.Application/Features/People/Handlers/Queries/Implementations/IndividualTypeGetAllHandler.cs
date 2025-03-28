﻿using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Implementations
{
    internal sealed class IndividualTypeGetAllHandler : IIndividualTypeGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualTypeQueryService _queryService;
        public IndividualTypeGetAllHandler(
            IMapper mapper,
            IIndividualTypeQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ICollection<IndividualTypeGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<IndividualType> individualType = await _queryService.Get();
            return _mapper.Map<ICollection<IndividualTypeGetDto>>(individualType);
        }
    }
}
