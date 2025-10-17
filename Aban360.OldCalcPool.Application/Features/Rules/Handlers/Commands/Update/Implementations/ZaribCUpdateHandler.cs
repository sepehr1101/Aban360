using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Implementations
{
    internal sealed class ZaribCUpdateHandler : IZaribCUpdateHandler
    {
        private readonly IZaribCCommandService _zaribCCommandService;
        private readonly IValidator<ZaribCUpdateDto> _zaribCUpdateValidator;

        public ZaribCUpdateHandler(
            IZaribCCommandService zaribCUpdateService,
            IValidator<ZaribCUpdateDto> ZaribCUpdateValidator)
        {
            _zaribCCommandService = zaribCUpdateService;
            _zaribCCommandService.NotNull(nameof(zaribCUpdateService));

            _zaribCUpdateValidator = ZaribCUpdateValidator;
            _zaribCUpdateValidator.NotNull(nameof(_zaribCUpdateValidator));
        }
        public async Task Handle(ZaribCUpdateDto UpdateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _zaribCUpdateValidator.ValidateAsync(UpdateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
            await _zaribCCommandService.Update(UpdateDto);
        }
    }
}
