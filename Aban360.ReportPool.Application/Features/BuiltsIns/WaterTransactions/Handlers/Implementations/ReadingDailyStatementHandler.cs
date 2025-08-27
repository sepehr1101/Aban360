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
    internal sealed class ReadingDailyStatementHandler : IReadingDailyStatementHandler
    {
        private readonly IReadingDailyStatementQueryService _readingDailyStatementQueryService;
        private readonly IValidator<ReadingDailyStatementInputDto> _validator;
        public ReadingDailyStatementHandler(
            IReadingDailyStatementQueryService readingDailyStatementQueryService,
            IValidator<ReadingDailyStatementInputDto> validator)
        {
            _readingDailyStatementQueryService = readingDailyStatementQueryService;
            _readingDailyStatementQueryService.NotNull(nameof(readingDailyStatementQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ReadingDailyStatementHeaderOutputDto, ReadingDailyStatementDataOutputDto>> Handle(ReadingDailyStatementInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<ReadingDailyStatementHeaderOutputDto, ReadingDailyStatementDataOutputDto> readingDailyStatement = await _readingDailyStatementQueryService.GetInfo(input);
            return readingDailyStatement;
        }
    }
}
