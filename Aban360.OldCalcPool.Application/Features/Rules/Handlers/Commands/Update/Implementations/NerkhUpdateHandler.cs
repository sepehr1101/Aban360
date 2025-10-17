using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Implementations
{
    internal sealed class NerkhUpdateHandler : INerkhUpdateHandler
    {
        private readonly INerkhUpdateService _nerkhUpdateService;
        private readonly IValidator<NerkhUpdateDto> _nerkhUpdateValidator;

        public NerkhUpdateHandler(
            INerkhUpdateService nerkhUpdateService,
            IValidator<NerkhUpdateDto> nerkhUpdateValidator)
        {
            _nerkhUpdateService = nerkhUpdateService;
            _nerkhUpdateService.NotNull(nameof(nerkhUpdateService));

            _nerkhUpdateValidator = nerkhUpdateValidator;
            _nerkhUpdateValidator.NotNull(nameof(_nerkhUpdateValidator));
        }
        public async Task Handle(NerkhUpdateDto UpdateDto, int nerkh, CancellationToken cancellationToken)
        {
            var validationResult = await _nerkhUpdateValidator.ValidateAsync(UpdateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
            await _nerkhUpdateService.Update(UpdateDto, nerkh);
        }
        public async Task Handle(NerkhUpdateDto UpdateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _nerkhUpdateValidator.ValidateAsync(UpdateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
            await _nerkhUpdateService.Update(UpdateDto);
        }
    }
}
