using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Implementations
{
    internal sealed class VirtualCategoryGetHandler : IVirtualCategoryGetHandler
    {
        private readonly IVirtualCategoryQueryService _virtualCategoryQueryService;
        private readonly IValidator<VirtualCategorySearchInputDto> _validator;

        public VirtualCategoryGetHandler(
            IVirtualCategoryQueryService virtualCategoryQueryService,
            IValidator<VirtualCategorySearchInputDto> validator)
        {
            _virtualCategoryQueryService = virtualCategoryQueryService;
            _virtualCategoryQueryService.NotNull(nameof(virtualCategoryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }
        public async Task<VirtualCategoryGetDto> Handle(VirtualCategorySearchInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            VirtualCategoryGetDto result = await _virtualCategoryQueryService.Get(input);
            return result;
        }
    }
}
