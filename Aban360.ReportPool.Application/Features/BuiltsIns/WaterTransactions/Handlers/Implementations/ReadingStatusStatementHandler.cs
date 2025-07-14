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
    internal sealed class ReadingStatusStatementHandler : IReadingStatusStatementHandler
    {
        private readonly IReadingStatusStatementQueryService _readingStatusStatementQueryService;
        private readonly IValidator<ReadingStatusStatementInputDto> _validator;
        public ReadingStatusStatementHandler(
            IReadingStatusStatementQueryService readingStatusStatementQueryService,
            IValidator<ReadingStatusStatementInputDto> validator)
        {
            _readingStatusStatementQueryService = readingStatusStatementQueryService;
            _readingStatusStatementQueryService.NotNull(nameof(readingStatusStatementQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementDataOutputDto>> Handle(ReadingStatusStatementInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementDataOutputDto> readingStatusStatement = await _readingStatusStatementQueryService.GetInfo(input);
            return readingStatusStatement;
        }
    }
}
