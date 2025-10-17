using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Implementations
{
    internal sealed class IndividualEstateCreateHandler : IIndividualEstateCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualEstateCommandService _commandService;
        private readonly IValidator<IndividualEstateCreateDto> _validator;
        public IndividualEstateCreateHandler(
            IMapper mapper,
            IIndividualEstateCommandService commandService,
            IValidator<IndividualEstateCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IndividualEstateCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            IndividualEstate individualEstate = _mapper.Map<IndividualEstate>(createDto);
            await _commandService.Add(individualEstate);
        }
    }
}
