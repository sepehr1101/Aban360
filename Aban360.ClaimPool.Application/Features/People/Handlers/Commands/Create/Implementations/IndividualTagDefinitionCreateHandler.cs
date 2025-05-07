using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Implementations
{
    internal sealed class IndividualTagDefinitionCreateHandler : IIndividualTagDefinitionCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualTagDefinitionCommandService _commandService;
        private readonly IIndividualTagDefinitionQueryService _queryService;
        private readonly IValidator<IndividualTagDefinitionCreateDto> _validator;
        public IndividualTagDefinitionCreateHandler(
            IMapper mapper,
            IIndividualTagDefinitionCommandService commandService,
            IIndividualTagDefinitionQueryService queryService,
            IValidator<IndividualTagDefinitionCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IndividualTagDefinitionCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            IndividualTagDefinition individualTagDefinition = _mapper.Map<IndividualTagDefinition>(createDto);
            await _commandService.Add(individualTagDefinition);
        }
    }
}
