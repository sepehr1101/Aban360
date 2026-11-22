using Aban360.Common.BaseEntities;
using Aban360.Common.Caching;
using Aban360.Common.Exceptions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using FluentValidation.Results;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations;
internal sealed class WaterReceivablesHandler : IWaterReceivablesHandler
{
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(2);
    private readonly IWaterReceivablesQueryService _queries;
    private readonly IValidator<WaterReceivablesInputDto> _validator;
    private readonly IQueryCache _cache;

    public WaterReceivablesHandler(
        IWaterReceivablesQueryService queries,
        IValidator<WaterReceivablesInputDto> validator,
        IQueryCache cache)
    {
        _queries = queries;
        _validator = validator;
        _cache = cache;
    }

    public async Task<ReportOutput<WaterReceivablesHeaderOutputDto, WaterReceivablesDataOutputDto>> Handle(WaterReceivablesInputDto input, CancellationToken cancellationToken)
    {
        ValidationResult validation = await _validator.ValidateAsync(input, cancellationToken);
        if (!validation.IsValid)
            throw new CustomValidationException(string.Join(", ", validation.Errors.Select(e => e.ErrorMessage)));

        int[]? zoneIds = input.ZoneIds is { Count: > 0 }
            ? input.ZoneIds.Distinct().Order().ToArray()
            : null;
        WaterReceivablesInputDto effectiveInput = input with { ZoneIds = zoneIds };
        ReportOutput<WaterReceivablesHeaderOutputDto, WaterReceivablesDataOutputDto> report = await _cache.GetOrCreateAsync(
            nameof(WaterReceivablesHandler), effectiveInput, CacheExpiration, CalculateAsync, cancellationToken);

        return new(report.Title,
            report.ReportHeader with { ReportDateJalali = DateTime.Now.ToShortPersianDateString() },
            report.ReportData.Select(row => row with { }).ToArray());
    }

    private async Task<ReportOutput<WaterReceivablesHeaderOutputDto, WaterReceivablesDataOutputDto>> CalculateAsync(
        WaterReceivablesInputDto input, CancellationToken cancellationToken)
    {
        const string historyStart = "1330/01/01";
        string openingEnd = input.FromDateJalali.ToGregorianDateOnly()!.Value.AddDays(-1)
            .ToDateTime(TimeOnly.MinValue).ToShortPersianDateString();
        Dictionary<string, WaterReceivablesDataOutputDto> rows = new();

        void Add(IEnumerable<WaterReceivablesAmountOutputDto> amounts, Action<WaterReceivablesDataOutputDto, long> update)
        {
            foreach (WaterReceivablesAmountOutputDto amount in amounts)
            {
                string title = amount.GroupTitle ?? string.Empty;
                if (!rows.TryGetValue(title, out WaterReceivablesDataOutputDto? row))
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
            Add(await _queries.GetSales(input.GroupBy, historyStart, openingEnd, input.ZoneIds, cancellationToken),
                (row, amount) => row.OpeningBalance += amount);
            Add(await _queries.GetCollections(input.GroupBy, historyStart, openingEnd, input.ZoneIds, cancellationToken),
                (row, amount) => row.OpeningBalance -= amount);
        }
        Add(await _queries.GetSales(input.GroupBy, input.FromDateJalali, input.ToDateJalali, input.ZoneIds, cancellationToken),
            (row, amount) => row.PeriodSales += amount);
        Add(await _queries.GetCollections(input.GroupBy, input.FromDateJalali, input.ToDateJalali, input.ZoneIds, cancellationToken),
            (row, amount) => row.PeriodCollections += amount);

        const string title = "گزارش مانده مطالبات آب‌بها";
        List<WaterReceivablesDataOutputDto> data = rows.Values.OrderBy(row => row.GroupTitle).ToList();
        WaterReceivablesHeaderOutputDto header = new()
        {
            Title = title,
            GroupBy = input.GroupBy,
            FromDateJalali = input.FromDateJalali,
            ToDateJalali = input.ToDateJalali,
            RecordCount = data.Count,
            OpeningBalance=data.Sum(data => data.OpeningBalance),
            PeriodCollections=data.Sum(data=>data.PeriodCollections),
            PeriodSales = data.Sum(data => data.PeriodSales)
        };
        return new ReportOutput<WaterReceivablesHeaderOutputDto, WaterReceivablesDataOutputDto>(title, header, data);
    }

}
