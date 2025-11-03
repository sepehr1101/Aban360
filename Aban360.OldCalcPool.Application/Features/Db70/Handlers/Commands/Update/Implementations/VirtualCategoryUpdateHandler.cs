using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Update.Implementations
{
    internal sealed class VirtualCategoryUpdateHandler : IVirtualCategoryUpdateHandler
    {
        private readonly IVirtualCategoryCommandService _virtualCategoryCommandService;
        private readonly IValidator<VirtualCategoryUpdateDto> _validator;

        public VirtualCategoryUpdateHandler(
            IVirtualCategoryCommandService virtualCategoryCommandService,
            IValidator<VirtualCategoryUpdateDto> validator)
        {
            _virtualCategoryCommandService = virtualCategoryCommandService;
            _virtualCategoryCommandService.NotNull(nameof(virtualCategoryCommandService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }
        public async Task Handle(VirtualCategoryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            await _virtualCategoryCommandService.Update(updateDto);
        }
    }
}
