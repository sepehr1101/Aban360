using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Delete.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Delete.Implementations
{
    internal sealed class VirtualCategoryDeleteHandler : IVirtualCategoryDeleteHandler
    {
        private readonly IVirtualCategoryCommandService _virtualCategoryCommandService;
        private readonly IValidator<SearchShortInputDto> _validator;

        public VirtualCategoryDeleteHandler(
            IVirtualCategoryCommandService virtualCategoryCommandService,
            IValidator<SearchShortInputDto> validator)
        {
            _virtualCategoryCommandService = virtualCategoryCommandService;
            _virtualCategoryCommandService.NotNull(nameof(virtualCategoryCommandService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }
        public async Task Handle(SearchShortInputDto deleteDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(deleteDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            await _virtualCategoryCommandService.Delete(deleteDto);
        }
    }
}
