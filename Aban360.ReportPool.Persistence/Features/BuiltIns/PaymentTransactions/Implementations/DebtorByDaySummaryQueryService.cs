using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class DebtorByDaySummaryQueryService : AbstractBaseConnection, IDebtorByDaySummaryQueryService
    {
        public DebtorByDaySummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDaySummaryDataOutputDto>> GetInfo(DebtorByDayInputDto input)
        {
            string debtorByDayQueryString = GetDebtorByDayDataQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<DebtorByDaySummaryDataOutputDto> debtorByDayData = await _sqlReportConnection.QueryAsync<DebtorByDaySummaryDataOutputDto>(debtorByDayQueryString, @params);
            DebtorByDayHeaderOutputDto debtorByDayHeader = new DebtorByDayHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = (debtorByDayData is not null && debtorByDayData.Any()) ? debtorByDayData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title= ReportLiterals.DebtorByDaySummary,

                CustomerCount = debtorByDayData.Sum(x => x.Count),
                SumAmount = debtorByDayData.Sum(x => x.Amount),
                SumFinalAmount = debtorByDayData.Sum(x => x.FinalAmount),
                SumOffAmount = debtorByDayData.Sum(x => x.OffAmount),
            };

            var result = new ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDaySummaryDataOutputDto>(ReportLiterals.DebtorByDaySummary, debtorByDayHeader, debtorByDayData);

            return result;
        }

        private string GetDebtorByDayDataQuery()
        {
            return @"Select
                    	r.RegisterDate AS EventDateJalali,
                    	COUNT(1) AS Count,
                    	SUM(r.Amount) AS Amount,
                    	SUM(r.OffAmount) AS OffAmount,
                    	Sum(r.FinalAmount) AS FinalAmount
                    From [CustomerWarehouse].dbo.RequestBillDetails r
                    Where	
                    	r.RegisterDate BETWEEN @fromDate AND @toDate AND
                    	r.ZoneId IN @zoneIds AND
                    	(r.TypeCode=1 OR r.TypeCode=2)
                    Group by r.RegisterDate";
        }

    }
}
