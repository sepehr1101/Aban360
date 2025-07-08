using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class DebtorByDayDetailQueryService : AbstractBaseConnection, IDebtorByDayDetailQueryService
    {
        public DebtorByDayDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDayDetailDataOutputDto>> GetInfo(DebtorByDayInputDto input)
        {
            string debtorByDayQueryString = GetDebtorByDayDataQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<DebtorByDayDetailDataOutputDto> debtorByDayData = await _sqlConnection.QueryAsync<DebtorByDayDetailDataOutputDto>(debtorByDayQueryString,@params);
            DebtorByDayHeaderOutputDto debtorByDayHeader = new DebtorByDayHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = debtorByDayData.Count(),
                ReportDate = DateTime.Now.ToShortPersianDateString(),

                SumAmount = debtorByDayData.Sum(x => x.Amount),
                SumFinalAmount = debtorByDayData.Sum(x => x.FinalAmount),
                SumOffAmount = debtorByDayData.Sum(x => x.OffAmount),
            };

            var result = new ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDayDetailDataOutputDto>(ReportLiterals.DebtorByDayDetail, debtorByDayHeader, debtorByDayData);

            return result;
        }

        private string GetDebtorByDayDataQuery()
        {
            return @"Select
                    	r.ItemTitle,
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
                    Group by
                    	r.ItemTitle,
                    	r.RegisterDate";
        }

    }
}
