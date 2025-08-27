using Aban360.Common.Excel;
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
    internal sealed class WithoutBillHandler : IWithoutBillHandler
    {
        private readonly IWithoutBillQueryService _withoutBillQueryService;
        private readonly IValidator<WithoutBillInputDto> _validator;
        public WithoutBillHandler(
            IWithoutBillQueryService withoutBillQueryService,
            IValidator<WithoutBillInputDto> validator)
        {
            _withoutBillQueryService = withoutBillQueryService;
            _withoutBillQueryService.NotNull(nameof(withoutBillQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto>> Handle(WithoutBillInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto> withoutBill = await _withoutBillQueryService.GetInfo(input);
            return withoutBill;
        }
    }
}
