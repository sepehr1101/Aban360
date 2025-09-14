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
    internal sealed class ReadingStatusStatementSummaryByZoneHandler : IReadingStatusStatementSummaryByZoneHandler
    {
        private readonly IReadingStatusStatementSummaryByZoneQueryService _readingStatusStatementSummaryByZoneQueryService;
        private readonly IValidator<ReadingStatusStatementInputDto> _validator;
        public ReadingStatusStatementSummaryByZoneHandler(
            IReadingStatusStatementSummaryByZoneQueryService readingStatusStatementSummaryByZoneQueryService,
            IValidator<ReadingStatusStatementInputDto> validator)
        {
            _readingStatusStatementSummaryByZoneQueryService = readingStatusStatementSummaryByZoneQueryService;
            _readingStatusStatementSummaryByZoneQueryService.NotNull(nameof(readingStatusStatementSummaryByZoneQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryByZoneDataOutputDto>> Handle(ReadingStatusStatementInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryByZoneDataOutputDto> readingStatusStatementSummaryByZone = await _readingStatusStatementSummaryByZoneQueryService.GetInfo(input);
            return readingStatusStatementSummaryByZone;
        }
    }
}
