using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class ReadingStatusStatementSummaryByUsageHandler : IReadingStatusStatementSummaryByUsageHandler
    {
        private readonly IReadingStatusStatementSummaryByUsageQueryService _readingStatusStatementSummaryByUsageQueryService;
        private readonly IValidator<ReadingStatusStatementInputDto> _validator;
        public ReadingStatusStatementSummaryByUsageHandler(
            IReadingStatusStatementSummaryByUsageQueryService readingStatusStatementSummaryByUsageQueryService,
            IValidator<ReadingStatusStatementInputDto> validator)
        {
            _readingStatusStatementSummaryByUsageQueryService = readingStatusStatementSummaryByUsageQueryService;
            _readingStatusStatementSummaryByUsageQueryService.NotNull(nameof(readingStatusStatementSummaryByUsageQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryDataOutputDto>> Handle(ReadingStatusStatementInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryDataOutputDto> readingStatusStatementSummaryByUsage = await _readingStatusStatementSummaryByUsageQueryService.GetInfo(input);
            return readingStatusStatementSummaryByUsage;
        }
    }
}
