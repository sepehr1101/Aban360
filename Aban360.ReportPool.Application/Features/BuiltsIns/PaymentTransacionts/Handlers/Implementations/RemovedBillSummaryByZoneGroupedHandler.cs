using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Base;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using FluentValidation;
using System.Runtime.InteropServices;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class RemovedBillSummaryByZoneGroupedHandler : IRemovedBillSummaryByZoneGroupedHandler
    {
        private readonly IRemovedBillSummaryByZoneQueryService _removedBillQueryService;
        private readonly IValidator<RemovedBillInputDto> _validator;
        public RemovedBillSummaryByZoneGroupedHandler(
            IRemovedBillSummaryByZoneQueryService removedBillQueryService,
            IValidator<RemovedBillInputDto> validator)
        {
            _removedBillQueryService = removedBillQueryService;
            _removedBillQueryService.NotNull(nameof(removedBillQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<RemovedBillHeaderOutputDto, ReportOutput<RemovedBillSummaryDataOutputDto, RemovedBillSummaryDataOutputDto>>> Handle(RemovedBillInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto> result = await _removedBillQueryService.GetInfo(input);

            var dataGroup = result.ReportData
                .GroupBy(m => m.RegionTitle)
                .Select(g =>
                {
                    var mapped = g.Select(MapToGroup);

                    return new ReportOutput
                    <RemovedBillSummaryDataOutputDto,
                    RemovedBillSummaryDataOutputDto>
                    (
                        result.Title,
                        ReportAggregator.AggregateGroup(mapped, g.Key),
                        mapped.Select(v => ReportAggregator.AggregateGroup(new[] { v }, v.ItemTitle))
                    );
                });
            ReportOutput<RemovedBillHeaderOutputDto, ReportOutput<RemovedBillSummaryDataOutputDto, RemovedBillSummaryDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGroup);

            return finalData;
        }
        public async Task<ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto>> HandleFlat(RemovedBillInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<RemovedBillHeaderOutputDto, ReportOutput<RemovedBillSummaryDataOutputDto, RemovedBillSummaryDataOutputDto>> result = await Handle(input, cancellationToken);

            ICollection<RemovedBillSummaryDataOutputDto> flatData = result
                .ReportData
                .SelectMany(f =>
                {
                    f.ReportHeader.IsFirstRow = true;
                    f.ReportData.Select(d => d.IsFirstRow = false);

                    return new[] { f.ReportHeader }.Concat(f.ReportData);
                }).ToList();

            ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto> flatResult = new(result.Title, result.ReportHeader, flatData) { };
            return flatResult;
        }

        private static RemovedBillSummaryDataOutputDto MapToGroup(RemovedBillSummaryDataOutputDto input)
        {
            return new RemovedBillSummaryDataOutputDto()
            {
                ItemTitle = input.ZoneTitle,
                Amount = input.Amount,
                AverageConsumption = input.AverageConsumption,
                CustomerCount = input.CustomerCount,
                SumConsumption = input.SumConsumption,
            };
        }
    }
}
