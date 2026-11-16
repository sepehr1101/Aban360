using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations;
internal sealed class WaterReceivablesHandler : IWaterReceivablesHandler
{
    private readonly IWaterReceivablesQueryService _queries;
    private readonly IValidator<WaterReceivablesInputDto> _validator;
    public WaterReceivablesHandler(IWaterReceivablesQueryService queries, IValidator<WaterReceivablesInputDto> validator)
    {
        _queries = queries;
        _validator = validator;
    }

    public async Task<ReportOutput<WaterReceivablesHeaderOutputDto, WaterReceivablesDataOutputDto>> Handle(WaterReceivablesInputDto input, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(input, cancellationToken);
        if (!validation.IsValid)
            throw new CustomValidationException(string.Join(", ", validation.Errors.Select(e => e.ErrorMessage)));

        const string historyStart = "1330/01/01";
        string openingEnd = input.FromDateJalali.ToGregorianDateOnly()!.Value.AddDays(-1)
            .ToDateTime(TimeOnly.MinValue).ToShortPersianDateString();
        var rows = new Dictionary<string, WaterReceivablesDataOutputDto>();

        void Add(IEnumerable<WaterReceivablesAmountOutputDto> amounts, Action<WaterReceivablesDataOutputDto, long> update)
        {
            foreach (var amount in amounts)
            {
                string title = amount.GroupTitle ?? string.Empty;
                if (!rows.TryGetValue(title, out var row))
                {
                    row = new WaterReceivablesDataOutputDto { GroupTitle = title };
                    rows.Add(title, row);
                }
                update(row, amount.Amount);
            }
        }

        // No opening transactions when the selected period starts at the history boundary.
        if (string.CompareOrdinal(input.FromDateJalali, historyStart) > 0)
        {
            Add(await _queries.GetSales(input.GroupBy, historyStart, openingEnd, cancellationToken),
                (row, amount) => row.OpeningBalance += amount);
            Add(await _queries.GetCollections(input.GroupBy, historyStart, openingEnd, cancellationToken),
                (row, amount) => row.OpeningBalance -= amount);
        }
        Add(await _queries.GetSales(input.GroupBy, input.FromDateJalali, input.ToDateJalali, cancellationToken),
            (row, amount) => row.PeriodSales += amount);
        Add(await _queries.GetCollections(input.GroupBy, input.FromDateJalali, input.ToDateJalali, cancellationToken),
            (row, amount) => row.PeriodCollections += amount);

        const string title = "گزارش مانده مطالبات آب‌بها";
        var data = rows.Values.OrderBy(row => row.GroupTitle).ToList();
        var header = new WaterReceivablesHeaderOutputDto
        {
            Title = title,
            GroupBy = input.GroupBy,
            FromDateJalali = input.FromDateJalali,
            ToDateJalali = input.ToDateJalali,
            ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
            RecordCount = data.Count
        };
        return new ReportOutput<WaterReceivablesHeaderOutputDto, WaterReceivablesDataOutputDto>(title, header, data);
    }
}
