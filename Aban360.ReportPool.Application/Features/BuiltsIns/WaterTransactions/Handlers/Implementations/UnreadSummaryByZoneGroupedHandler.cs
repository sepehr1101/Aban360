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
                throw new CustomValidationException(message);
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
        public async Task<ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryDataOutputDto>> HandleFlat(UnreadInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<UnreadSummaryHeaderOutputDto, ReportOutput<UnreadSummaryDataOutputDto, UnreadSummaryDataOutputDto>> result = await Handle(input, cancellationToken);

            ICollection<UnreadSummaryDataOutputDto> flatData = result
                .ReportData
                .SelectMany(f =>
                {
                    f.ReportHeader.IsFirstRow = true;
                    f.ReportData.Select(d => d.IsFirstRow = false);

                    return new[] { f.ReportHeader }.Concat(f.ReportData);
                }).ToList();

            ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryDataOutputDto> flatResult = new(result.Title, result.ReportHeader, flatData) { };
            return flatResult;
        }
        private static UnreadSummaryDataOutputDto MapToGrouped(UnreadSummaryByZoneDataOutputDto input)
        {
            return new UnreadSummaryDataOutputDto()
            {
                ItemTitle = input.ZoneTitle,
                Barrier = input.Barrier,
                Closed = input.Closed,
                CustomerCount = input.CustomerCount,
                CommercialUnit = input.CommercialUnit,
                DomesticUnit = input.DomesticUnit,
                TotalUnit=input.TotalUnit,  
                OtherUnit = input.OtherUnit,
                UnSpecified = input.UnSpecified,
                Field0_5 = input.Field0_5,
                Field0_75 = input.Field0_75,
                Field1 = input.Field1,
                Field1_2 = input.Field1_2,
                Field1_5 = input.Field1_5,
                Field2 = input.Field2,
                Field3 = input.Field3,
                Field4 = input.Field4,
                Field5 = input.Field5,
                MoreThan6 = input.MoreThan6
            };
        }
    }
}
