using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Implementations
{
    public interface IPreviousAverageHandler
    {
        Task<float> HandleByLatestReading(int zoneId, int customerNumber, string from, string to);
        Task<float?> HandleByPreviousYear(int zoneId, int customerNumber, string from, string to);
    }

    internal sealed class PreviousAverageHandler : IPreviousAverageHandler
    {
        private readonly IBedBesQueryService _bedBesQueryService;
        public PreviousAverageHandler(IBedBesQueryService bedBesQueryService)
        {
            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(_bedBesQueryService));
        }

        public async Task<float?> HandleByPreviousYear(int zoneId, int customerNumber, string @from, string @to)
        {
            float? previousConsumptionAverage = await _bedBesQueryService.GetAverage(zoneId, customerNumber, GetPreviousYear(from), GetPreviousYear(to));
            return previousConsumptionAverage;
        }
        public async Task<float> HandleByLatestReading(int zoneId, int customerNumber, string @from, string @to)
        {
            float previousConsumptionAverage = await _bedBesQueryService.GetPreviousBill(zoneId, customerNumber, from);
            return previousConsumptionAverage;
        }
        private string GetPreviousYear(string dateJalali)
        {
            DateOnly? dateOnly = dateJalali.ToGregorianDateOnly();
            if (!dateOnly.HasValue)
            {
                throw new BaseException("تاریخ ناصحیح است");
            }
            string previousDateJalali = dateOnly.Value.AddYears(-1).ToShortPersianDateString();
            return previousDateJalali;
        }

    }
}
