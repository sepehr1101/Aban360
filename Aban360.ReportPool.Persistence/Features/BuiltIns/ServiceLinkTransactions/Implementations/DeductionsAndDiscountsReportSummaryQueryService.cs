using Aban360.Common.BaseEntities;
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
            IEnumerable<DeductionsAndDiscountsReportSummaryDataOutputDto> deductionsAndDiscountsReportData = await _sqlReportConnection.QueryAsync<DeductionsAndDiscountsReportSummaryDataOutputDto>(deductionsAndDiscountsReportQueryString, @params);
            DeductionsAndDiscountsReportHeaderOutputDto deductionsAndDiscountsReportHeader = new DeductionsAndDiscountsReportHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                CustomerCount = (deductionsAndDiscountsReportData is not null && deductionsAndDiscountsReportData.Any()) ? deductionsAndDiscountsReportData.Count() : 0,
                RecordCount = (deductionsAndDiscountsReportData is not null && deductionsAndDiscountsReportData.Any()) ? deductionsAndDiscountsReportData.Count() : 0,
                TotalOffAmount=deductionsAndDiscountsReportData.Sum(x=>x.SumOffAmount),
                Title= ReportLiterals.DeductionsAndDiscountsReportSummary,
            };

            var result = new ReportOutput<DeductionsAndDiscountsReportHeaderOutputDto, DeductionsAndDiscountsReportSummaryDataOutputDto>(ReportLiterals.DeductionsAndDiscountsReportSummary, deductionsAndDiscountsReportHeader, deductionsAndDiscountsReportData);

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
