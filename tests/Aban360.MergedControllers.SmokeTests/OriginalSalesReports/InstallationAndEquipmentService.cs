using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Exceptions;

namespace BeforeSalesReports.Aban360.CalculationPool.Application.Features.Sale.Services{
    public interface IInstallationAndEquipmentService
    {
        Task<IEnumerable<InstallationAndEquipmentOutputDto>> InstallationAndEquipmentGetAllAsync(CancellationToken cancellationToken);
        Task<InstallationAndEquipmentOutputDto> InstallationAndEquipmentGetAsync(SearchById input, CancellationToken cancellationToken);
    }

    internal sealed class InstallationAndEquipmentService     {
        private readonly IInstallationAndEquipmentQueryService _queryService;
        private readonly IValidator<SearchById> _validator;
        public InstallationAndEquipmentService(IInstallationAndEquipmentQueryService queryService, IValidator<SearchById> validator)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<IEnumerable<InstallationAndEquipmentOutputDto>> InstallationAndEquipmentGetAllAsync(CancellationToken cancellationToken)
        {
            IEnumerable<InstallationAndEquipmentOutputDto> result = await _queryService.Get(DateTime.Now.ToShortPersianDateString());
            return result;
        }

        public async Task<InstallationAndEquipmentOutputDto> InstallationAndEquipmentGetAsync(SearchById input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            InstallationAndEquipmentOutputDto result = await _queryService.Get(input.Id, DateTime.Now.ToShortPersianDateString());
            return result;
        }
    }
}
