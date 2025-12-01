using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Implementations
{
    internal sealed class SCreateHandler : ISCreateHandler
    {
        private readonly ISCommandService _sCommandService;
        private readonly IValidator<SCreateDto> _validator;

        public SCreateHandler(
            ISCommandService sCreateService,
            IValidator<SCreateDto> validator)
        {
            _sCommandService = sCreateService;
            _sCommandService.NotNull(nameof(sCreateService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }
        public async Task Handle(SCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
            await _sCommandService.Create(createDto);
        }
    }
}
