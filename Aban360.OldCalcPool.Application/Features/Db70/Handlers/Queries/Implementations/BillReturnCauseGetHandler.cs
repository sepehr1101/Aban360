using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Implementations
{
    internal sealed class BillReturnCauseGetHandler : IBillReturnCauseGetHandler
    {
        private readonly IBillReturnCauseQueryService _virtualCategoryQueryService;
        private readonly IValidator<SearchShortInputDto> _validator;

        public BillReturnCauseGetHandler(
            IBillReturnCauseQueryService virtualCategoryQueryService,
            IValidator<SearchShortInputDto> validator)
        {
            _virtualCategoryQueryService = virtualCategoryQueryService;
            _virtualCategoryQueryService.NotNull(nameof(virtualCategoryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }
        public async Task<BillReturnCauseGetDto> Handle(SearchShortInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            BillReturnCauseGetDto result = await _virtualCategoryQueryService.Get(input);
            return result;
        }
    }
}
