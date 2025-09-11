using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using FluentValidation;
using System.Threading;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Implementations
{
    internal sealed class NerkhCreateHandler : INerkhCreateHandler
    {
        private readonly INerkhCreateService _nerkhCreateService;
        private readonly IValidator<NerkhCreateDto> _nerkhCreateValidator;

        public NerkhCreateHandler(
            INerkhCreateService nerkhCreateService,
            IValidator<NerkhCreateDto> nerkhCreateValidator)
        {
            _nerkhCreateService = nerkhCreateService;
            _nerkhCreateService.NotNull(nameof(nerkhCreateService));

            _nerkhCreateValidator = nerkhCreateValidator;
            _nerkhCreateValidator.NotNull(nameof(_nerkhCreateValidator));
        }
        public async Task Handle(NerkhCreateDto createDto, int nerkh, CancellationToken cancellationToken)
        {
            var validationResult = await _nerkhCreateValidator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }
            await _nerkhCreateService.Create(createDto, nerkh);
        }
        public async Task Handle(NerkhCreateDto createDto,  CancellationToken cancellationToken)
        {
            var validationResult = await _nerkhCreateValidator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }
            await _nerkhCreateService.Create(createDto);
        }
    }
}
