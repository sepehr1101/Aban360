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
    internal sealed class UnreadSummaryByZoneGroupedHandler : IUnreadSummaryByZoneGroupedHandler
    {
        private readonly IUnreadSummaryByZoneQueryService _unreadSummaryByZoneQueryService;
        private readonly IValidator<UnreadInputDto> _validator;
        public UnreadSummaryByZoneGroupedHandler(
            IUnreadSummaryByZoneQueryService unreadSummaryByZoneQueryService,
            IValidator<UnreadInputDto> validator)
        {
            _unreadSummaryByZoneQueryService = unreadSummaryByZoneQueryService;
            _unreadSummaryByZoneQueryService.NotNull(nameof(unreadSummaryByZoneQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<UnreadSummaryHeaderOutputDto, ReportOutput<UnreadSummaryDataOutputDto, UnreadSummaryDataOutputDto>>> Handle(UnreadInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryByZoneDataOutputDto> result = await _unreadSummaryByZoneQueryService.GetInfo(input);

            var dataGroup = result.ReportData
                 .GroupBy(m => m.RegionTitle)
                 .Select(g =>
                 {
                     var mapped = g.Select(MapToGrouped);
                     return new ReportOutput<
                         UnreadSummaryDataOutputDto,
                         UnreadSummaryDataOutputDto>
                     (
                         result.Title,
                         ReportAggregator.AggregateGroup(mapped, g.Key),
                         mapped.Select(v => ReportAggregator.AggregateGroup(new[] { v }, v.ItemTitle))

                     );
                 });

            ReportOutput<UnreadSummaryHeaderOutputDto, ReportOutput<UnreadSummaryDataOutputDto, UnreadSummaryDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGroup);

            return finalData;
        }

        private static UnreadSummaryDataOutputDto MapToGrouped(UnreadSummaryByZoneDataOutputDto input)
        {
            return new UnreadSummaryDataOutputDto()
            {
                ItemTitle = input.ZoneTitle,
                Barrier = input.Barrier,
                Closed = input.Closed,
            };
        }
    }
}
