using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Base;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class ReadingListSummaryByZoneGroupedHandler : IReadingListSummaryByZoneGroupedHandler
    {
        private readonly IReadingListSummaryByZoneQueryService _readingListSummaryQuery;
        private readonly IValidator<ReadingListInputDto> _validator;
        public ReadingListSummaryByZoneGroupedHandler(
            IReadingListSummaryByZoneQueryService readingListSummaryQuery,
            IValidator<ReadingListInputDto> validator)
        {
            _readingListSummaryQuery = readingListSummaryQuery;
            _readingListSummaryQuery.NotNull(nameof(readingListSummaryQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ReadingListHeaderOutputDto, ReportOutput<ReadingListSummaryDataOutputDto, ReadingListSummaryDataOutputDto>>> Handle(ReadingListInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<ReadingListHeaderOutputDto, ReadingListSummaryDataOutputDto> result = await _readingListSummaryQuery.GetInfo(input);

            var dataGroup = result.ReportData
                .GroupBy(m => m.RegionTitle)
                .Select(g =>
                {
                    var mapped = g.Select(MapToGroup);
                    return new ReportOutput
                    <ReadingListSummaryDataOutputDto,
                    ReadingListSummaryDataOutputDto>
                    (
                        result.Title,
                        ReportAggregator.AggregateGroup(mapped, g.Key),
                        mapped.Select(v => ReportAggregator.AggregateGroup(new[] { v }, v.ItemTitle))
                    );
                });
            ReportOutput<ReadingListHeaderOutputDto, ReportOutput<ReadingListSummaryDataOutputDto, ReadingListSummaryDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGroup);

            return finalData;
        }
        public async Task<ReportOutput<ReadingListHeaderOutputDto, ReadingListSummaryDataOutputDto>> HandleFlat(ReadingListInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<ReadingListHeaderOutputDto, ReportOutput<ReadingListSummaryDataOutputDto, ReadingListSummaryDataOutputDto>> result = await Handle(input, cancellationToken);

            ICollection<ReadingListSummaryDataOutputDto> flatData = result
                .ReportData
                .SelectMany(f =>
                {
                    f.ReportHeader.IsFirstRow = true;
                    f.ReportData.Select(d => d.IsFirstRow = false);

                    return new[] { f.ReportHeader }.Concat(f.ReportData);
                }).ToList();

            ReportOutput<ReadingListHeaderOutputDto, ReadingListSummaryDataOutputDto> flatResult = new(result.Title, result.ReportHeader, flatData) { };
            return flatResult;
        }

        private static ReadingListSummaryDataOutputDto MapToGroup(ReadingListSummaryDataOutputDto input)
        {
            return new ReadingListSummaryDataOutputDto()
            {
                ItemTitle = input.ZoneTitle,
                AdvancePaymentCount = input.AdvancePaymentCount,
                CloseCount = input.CloseCount,
                MalfunctionCount = input.MalfunctionCount,
                ObstacleCount = input.ObstacleCount,
                ReadingCount = input.ReadingCount,
                ReplacementBranchCount = input.ReplacementBranchCount,
            };
        }
    }
}
