﻿using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Implementations
{
    internal sealed class IndividualTagUpdateHandler : IIndividualTagUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualTagQueryService _IndividualTagQueryService;
        private readonly IValidator<IndividualTagUpdateDto> _validator;
        public IndividualTagUpdateHandler(
            IMapper mapper,
            IIndividualTagQueryService IndividualTagQueryService,
            IValidator<IndividualTagUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _IndividualTagQueryService = IndividualTagQueryService;
            _IndividualTagQueryService.NotNull(nameof(IndividualTagQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IndividualTagUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            IndividualTag individualTag = await _IndividualTagQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, individualTag);
        }
    }
}
