using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class RemovedBillSummaryByUsageHandler : IRemovedBillSummaryByUsageHandler
    {
        private readonly IRemovedBillSummaryByUsageQueryService _removedBillQueryService;
        private readonly IValidator<RemovedBillInputDto> _validator;
        public RemovedBillSummaryByUsageHandler(
            IRemovedBillSummaryByUsageQueryService removedBillQueryService,
            IValidator<RemovedBillInputDto> validator)
        {
            _removedBillQueryService = removedBillQueryService;
            _removedBillQueryService.NotNull(nameof(removedBillQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto>> Handle(RemovedBillInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto> removedBill = await _removedBillQueryService.GetInfo(input);
            return removedBill;
        }
    }
}
