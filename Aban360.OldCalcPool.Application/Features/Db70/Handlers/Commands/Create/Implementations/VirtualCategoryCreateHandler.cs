using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Create.Implementations
{
    internal sealed class VirtualCategoryCreateHandler : IVirtualCategoryCreateHandler
    {
        private readonly IVirtualCategoryCommandService _virtualCategoryCommandService;
        private readonly IValidator<VirtualCategoryCreateDto> _validator;

        public VirtualCategoryCreateHandler(
            IVirtualCategoryCommandService virtualCategoryCommandService,
            IValidator<VirtualCategoryCreateDto> validator)
        {
            _virtualCategoryCommandService = virtualCategoryCommandService;
            _virtualCategoryCommandService.NotNull(nameof(virtualCategoryCommandService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }
        public async Task Handle(VirtualCategoryCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            await _virtualCategoryCommandService.Create(createDto);
        }
    }
}