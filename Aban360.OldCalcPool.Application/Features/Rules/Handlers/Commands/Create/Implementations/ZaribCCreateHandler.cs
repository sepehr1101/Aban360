using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Implementations
{
    internal sealed class ZaribCCreateHandler : IZaribCCreateHandler
    {
        private readonly IZaribCCommandService _zaribCCommandService;
        private readonly IValidator<ZaribCCreateDto> _zaribCCreateValidator;

        public ZaribCCreateHandler(
            IZaribCCommandService zaribCCreateService,
            IValidator<ZaribCCreateDto> ZaribCCreateValidator)
        {
            _zaribCCommandService = zaribCCreateService;
            _zaribCCommandService.NotNull(nameof(zaribCCreateService));

            _zaribCCreateValidator = ZaribCCreateValidator;
            _zaribCCreateValidator.NotNull(nameof(_zaribCCreateValidator));
        }
        public async Task Handle(ZaribCCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _zaribCCreateValidator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
            await _zaribCCommandService.Create(createDto);
        }
    }
}
