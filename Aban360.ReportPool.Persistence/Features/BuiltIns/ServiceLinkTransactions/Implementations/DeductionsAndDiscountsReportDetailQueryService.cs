using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class DeductionsAndDiscountsReportDetailQueryService : AbstractBaseConnection, IDeductionsAndDiscountsReportDetailQueryService
    {
        public DeductionsAndDiscountsReportDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<DeductionsAndDiscountsReportHeaderOutputDto, DeductionsAndDiscountsReportDetailDataOutputDto>> GetInfo(DeductionsAndDiscountsReportInputDto input)
        {
            string deductionsAndDiscountsReportQueryString = GetDeductionsAndDiscountsReportDataQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds= input.ZoneIds,
            };
            IEnumerable<DeductionsAndDiscountsReportDetailDataOutputDto> deductionsAndDiscountsReportData = await _sqlConnection.QueryAsync<DeductionsAndDiscountsReportDetailDataOutputDto>(deductionsAndDiscountsReportQueryString,@params);
            DeductionsAndDiscountsReportHeaderOutputDto deductionsAndDiscountsReportHeader = new DeductionsAndDiscountsReportHeaderOutputDto()
            {
                FromDateJalali=input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDate=DateTime.Now.ToShortPersianDateString(),
                RecordCount=deductionsAndDiscountsReportData.Count(),
                
                TotalOffAmount=deductionsAndDiscountsReportData.Sum(data=>data.OffAmount),
            };

            var result = new ReportOutput<DeductionsAndDiscountsReportHeaderOutputDto, DeductionsAndDiscountsReportDetailDataOutputDto>(ReportLiterals.DeductionsAndDiscountsReport, deductionsAndDiscountsReportHeader, deductionsAndDiscountsReportData);

            return result;
        }

        private string GetDeductionsAndDiscountsReportDataQuery()
        {
            return @"Select 
                    	r.ItemTitle AS DiscountTypeTitle,
                    	r.OffTitle AS OffTypeTitle,
                    	r.TypeId AS ToDiscountTitle,
                    	r.OffAmount AS OffAmount
                    From [CustomerWarehouse].dbo.RequestBillDetails r
                    Where
                    	(r.RegisterDate BETWEEN @fromDate AND @toDate)  AND
                    	r.ZoneId IN @zoneIds AND
                    	r.OffAmount > 0";
        }

    }
}
