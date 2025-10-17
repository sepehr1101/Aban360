using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Implementations
{
    internal sealed class ZaribUpdateHandler : IZaribUpdateHandler
    {
        private readonly IZaribUpdateService _zaribUpdateService;
        private readonly IValidator<ZaribUpdateDto> _ZaribUpdateValidator;

        public ZaribUpdateHandler(
            IZaribUpdateService zaribUpdateService,
            IValidator<ZaribUpdateDto> zaribUpdateValidator)
        {
            _zaribUpdateService = zaribUpdateService;
            _zaribUpdateService.NotNull(nameof(zaribUpdateService));

            _ZaribUpdateValidator = zaribUpdateValidator;
            _ZaribUpdateValidator.NotNull(nameof(_ZaribUpdateValidator));
        }
        public async Task Handle(ZaribUpdateDto UpdateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _ZaribUpdateValidator.ValidateAsync(UpdateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
            await _zaribUpdateService.Update(UpdateDto);
        }
    }
}
