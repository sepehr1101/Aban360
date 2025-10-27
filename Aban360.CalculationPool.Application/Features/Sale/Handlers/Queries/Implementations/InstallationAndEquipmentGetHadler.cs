using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    internal sealed class InstallationAndEquipmentGetHadler : IInstallationAndEquipmentGetHadler
    {
        private readonly IInstallationAndEquipmentQueryService _queryService;
        private readonly IValidator<SearchById> _validator;
        public InstallationAndEquipmentGetHadler(
            IInstallationAndEquipmentQueryService queryService,
            IValidator<SearchById> validator)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<InstallationAndEquipmentOutputDto> Handle(SearchById input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            InstallationAndEquipmentOutputDto result = await _queryService.Get(input.Id);
            return result;
        }
    }
}
