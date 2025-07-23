using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Implementations
{
    internal sealed class ZaribCreateHandler : IZaribCreateHandler
    {
        private readonly IZaribCreateService _zaribCreateService;
        private readonly IValidator<ZaribCreateDto> _zaribCreateValidator;

        public ZaribCreateHandler(
            IZaribCreateService zaribCreateService,
            IValidator<ZaribCreateDto> ZaribCreateValidator)
        {
            _zaribCreateService = zaribCreateService;
            _zaribCreateService.NotNull(nameof(zaribCreateService));

            _zaribCreateValidator = ZaribCreateValidator;
            _zaribCreateValidator.NotNull(nameof(_zaribCreateValidator));
        }
        public async Task Handle(ZaribCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _zaribCreateValidator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }
            await _zaribCreateService.Create(createDto);
        }
    }
}
