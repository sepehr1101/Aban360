﻿using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestIndividualEstateCreateHandler : IRequestIndividualEstateCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualEstateCommandService _requestIndividualEstateCommandService;
        public RequestIndividualEstateCreateHandler(
            IMapper mapper,
            IRequestIndividualEstateCommandService requestIndividualEstateCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualEstateCommandService = requestIndividualEstateCommandService;
            _requestIndividualEstateCommandService.NotNull(nameof(_requestIndividualEstateCommandService));
        }

        public async Task Handle(IndividualEstateRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var requestIndividualEstate = _mapper.Map<RequestIndividualEstate>(createDto);
            await _requestIndividualEstateCommandService.Add(requestIndividualEstate);
        }
    }
}
