﻿using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class ReadingListDetailHandler : IReadingListDetailHandler
    {
        private readonly IReadingListDetailQueryService _readingListDetailQuery;
        private readonly IValidator<ReadingListInputDto> _validator;
        public ReadingListDetailHandler(
            IReadingListDetailQueryService readingListDetailQuery,
            IValidator<ReadingListInputDto> validator)
        {
            _readingListDetailQuery = readingListDetailQuery;
            _readingListDetailQuery.NotNull(nameof(readingListDetailQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ReadingListHeaderOutputDto, ReadingListDetailDataOutputDto>> Handle(ReadingListInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var result = await _readingListDetailQuery.GetInfo(input);
            return result;
        }
    }
}
