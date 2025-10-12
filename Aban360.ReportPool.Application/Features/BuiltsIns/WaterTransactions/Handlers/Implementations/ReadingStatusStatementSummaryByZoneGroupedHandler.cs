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
    internal sealed class ReadingStatusStatementSummaryByZoneGroupedHandler : IReadingStatusStatementSummaryByZoneGroupedHandler
    {
        private readonly IReadingStatusStatementSummaryByZoneQueryService _readingStatusStatementSummaryByZoneQueryService;
        private readonly IValidator<ReadingStatusStatementInputDto> _validator;
        public ReadingStatusStatementSummaryByZoneGroupedHandler(
            IReadingStatusStatementSummaryByZoneQueryService readingStatusStatementSummaryByZoneQueryService,
            IValidator<ReadingStatusStatementInputDto> validator)
        {
            _readingStatusStatementSummaryByZoneQueryService = readingStatusStatementSummaryByZoneQueryService;
            _readingStatusStatementSummaryByZoneQueryService.NotNull(nameof(readingStatusStatementSummaryByZoneQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ReadingStatusStatementHeaderOutputDto, ReportOutput<ReadingStatusStatementSummaryDataOutputDto, ReadingStatusStatementSummaryDataOutputDto>>> Handle(ReadingStatusStatementInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryDataOutputDto> result = await _readingStatusStatementSummaryByZoneQueryService.GetInfo(input);

            var dataGroup = result.ReportData
                .GroupBy(m => m.RegionTitle)
                .Select(g =>
                {
                    var mapped = g.Select(MapToGroup);
                    return new ReportOutput
                    <ReadingStatusStatementSummaryDataOutputDto,
                    ReadingStatusStatementSummaryDataOutputDto>
                    (
                        result.Title,
                        ReportAggregator.AggregateGroup(mapped, g.Key),
                        mapped.Select(v => ReportAggregator.AggregateGroup(new[] { v }, v.ZoneTitle))
                    );
                });

            ReportOutput<ReadingStatusStatementHeaderOutputDto, ReportOutput<ReadingStatusStatementSummaryDataOutputDto, ReadingStatusStatementSummaryDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGroup);

            return finalData;
        }
        public async Task<ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryDataOutputDto>> HandleFlat(ReadingStatusStatementInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<ReadingStatusStatementHeaderOutputDto, ReportOutput<ReadingStatusStatementSummaryDataOutputDto, ReadingStatusStatementSummaryDataOutputDto>> result = await Handle(input, cancellationToken);

            ICollection<ReadingStatusStatementSummaryDataOutputDto> flatData = result
                .ReportData
                .SelectMany(f =>
                {
                    f.ReportHeader.IsFirstRow = true;
                    f.ReportData.Select(d => d.IsFirstRow = false);

                    return new[] { f.ReportHeader }.Concat(f.ReportData);
                }).ToList();

            ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryDataOutputDto> flatResult = new(result.Title, result.ReportHeader, flatData) { };
            return flatResult;
        }

        private static ReadingStatusStatementSummaryDataOutputDto MapToGroup(ReadingStatusStatementSummaryDataOutputDto input)
        {
            return new ReadingStatusStatementSummaryDataOutputDto()
            {
                ZoneTitle = input.ZoneTitle,
                AllCount = input.AllCount,
                Closed = input.Closed,
                Obstacle = input.Obstacle,
                PureReading = input.PureReading,
                Ruined = input.Ruined,
                Temporarily = input.Temporarily,
                SumItems=input.SumItems,
                SelfClaimedCount=input.SelfClaimedCount,
                Debt=input.Debt
            };
        }
    }
}
