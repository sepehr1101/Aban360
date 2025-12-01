using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Implementations
{
    internal sealed class SUpdateHandler : ISUpdateHandler
    {
        private readonly ISCommandService _sCommandService;
        private readonly IValidator<SUpdateDto> _validator;

        public SUpdateHandler(
            ISCommandService sUpdateService,
            IValidator<SUpdateDto> validator)
        {
            _sCommandService = sUpdateService;
            _sCommandService.NotNull(nameof(sUpdateService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }
        public async Task Handle(SUpdateDto UpdateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(UpdateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
            await _sCommandService.Update(UpdateDto);
        }
    }
}
