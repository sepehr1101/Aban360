using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
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
    internal sealed class IndividualTagDefinitionUpdateHandler : IIndividualTagDefinitionUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualTagDefinitionQueryService _queryService;
        private readonly IValidator<IndividualTagDefinitionUpdateDto> _validator;
        public IndividualTagDefinitionUpdateHandler(
            IMapper mapper,
            IIndividualTagDefinitionQueryService queryService,
            IValidator<IndividualTagDefinitionUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IndividualTagDefinitionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            IndividualTagDefinition individualTagDefinition = await _queryService.Get(updateDto.Id);
            _mapper.Map(updateDto, individualTagDefinition);
        }
    }
}
