using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class DeductionsAndDiscountsReportSummaryQueryService : AbstractBaseConnection, IDeductionsAndDiscountsReportSummaryQueryService
    {
        public DeductionsAndDiscountsReportSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<DeductionsAndDiscountsReportHeaderOutputDto, DeductionsAndDiscountsReportSummaryDataOutputDto>> GetInfo(DeductionsAndDiscountsReportInputDto input)
        {
            string deductionsAndDiscountsReportQueryString = GetDeductionsAndDiscountsReportDataQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<DeductionsAndDiscountsReportSummaryDataOutputDto> deductionsAndDiscountsReportData = await _sqlConnection.QueryAsync<DeductionsAndDiscountsReportSummaryDataOutputDto>(deductionsAndDiscountsReportQueryString, @params);
            DeductionsAndDiscountsReportHeaderOutputDto deductionsAndDiscountsReportHeader = new DeductionsAndDiscountsReportHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDate = DateTime.Now.ToShortPersianDateString(),
                RecordCount = deductionsAndDiscountsReportData.Count(),

            };

            var result = new ReportOutput<DeductionsAndDiscountsReportHeaderOutputDto, DeductionsAndDiscountsReportSummaryDataOutputDto>(ReportLiterals.DeductionsAndDiscountsReport, deductionsAndDiscountsReportHeader, deductionsAndDiscountsReportData);

            return result;
        }

        private string GetDeductionsAndDiscountsReportDataQuery()
        {
            return @"Select 
                    	r.ItemTitle AS DiscountTypeTitle,
                    	SUM(r.OffAmount ) AS SumOffAmount
                    From [CustomerWarehouse].dbo.RequestBillDetails r
                    Where
                    	(r.RegisterDate BETWEEN @fromDate AND @toDate)  AND
                    	r.ZoneId IN @zoneIds AND
                    	r.OffAmount > 0
                    Group By r.ItemTitle";
        }

    }
}
