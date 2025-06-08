using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class ModifiedBillsHandler : IModifiedBillsHandler
    {
        private readonly IModifiedBillsQueryService _modifiedBillsQueryService;
        private readonly IValidator<ModifiedBillsInputDto> _validator;
        public ModifiedBillsHandler(
            IModifiedBillsQueryService modifiedBillsQueryService,
            IValidator<ModifiedBillsInputDto> validator)
        {
            _modifiedBillsQueryService = modifiedBillsQueryService;
            _modifiedBillsQueryService.NotNull(nameof(modifiedBillsQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ModifiedBillsHeaderOutputDto, ModifiedBillsDataOutputDto>> Handle(ModifiedBillsInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<ModifiedBillsHeaderOutputDto, ModifiedBillsDataOutputDto> modifiedBills = await _modifiedBillsQueryService.GetInfo(input);
            return modifiedBills;
        }
    }
}
