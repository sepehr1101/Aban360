using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class ProfessionCreateHandler : IProfessionCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IProfessionCommandService _professionCommandService;
        private readonly IValidator<ProfessionCreateDto> _validator;

        public ProfessionCreateHandler(
            IMapper mapper,
            IProfessionCommandService professionCommandService,
            IValidator<ProfessionCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _professionCommandService = professionCommandService;
            _professionCommandService.NotNull(nameof(_professionCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(ProfessionCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            Profession profession = _mapper.Map<Profession>(createDto);
            await _professionCommandService.Add(profession);
        }
    }
}
