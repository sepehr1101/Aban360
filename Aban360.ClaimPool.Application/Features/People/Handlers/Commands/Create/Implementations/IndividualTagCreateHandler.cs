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
    internal sealed class IndividualTagCreateHandler : IIndividualTagCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualTagCommandService _individualTagCommandService;
        private readonly IValidator<IndividualTagCreateDto> _validator;
        public IndividualTagCreateHandler(
            IMapper mapper,
            IIndividualTagCommandService IndividualTagCommandService,
            IValidator<IndividualTagCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _individualTagCommandService = IndividualTagCommandService;
            _individualTagCommandService.NotNull(nameof(IndividualTagCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IndividualTagCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            IndividualTag individualTag = _mapper.Map<IndividualTag>(createDto);
            individualTag.InsertLogInfo = " ";//todo
            individualTag.ValidFrom=DateTime.Now;
            await _individualTagCommandService.Add(individualTag);
        }
    }
}
