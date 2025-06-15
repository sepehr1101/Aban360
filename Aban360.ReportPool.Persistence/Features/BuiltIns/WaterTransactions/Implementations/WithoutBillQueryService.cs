using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WithoutBillQueryService : AbstractBaseConnection, IWithoutBillQueryService
    {
        public WithoutBillQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto>> GetInfo(WithoutBillInputDto input)
        {
            string withoutBill = GetWithoutBillQuery(input.ZoneIds?.Any()==true);
            var @params = new
            { 
                @FromDate=input.FromDateJalali,
                @ToDate=input.ToDateJalali,
                @FromReadingNumber=input.FromReadingNumber,
                @ToReadingNumber=input.ToReadingNumber,
                @ZoneIds=input.ZoneIds,
            };

            IEnumerable<WithoutBillDataOutputDto> withoutBillData = await _sqlReportConnection.QueryAsync<WithoutBillDataOutputDto>(withoutBill);
            WithoutBillHeaderOutputDto withoutBillHeader = new WithoutBillHeaderOutputDto()
            {
                FromDateTime=input.FromDateJalali,
                ToDateTime=input.ToDateJalali,
                FromReadingNumber=input.FromReadingNumber,
                ToReadingNumber=input.ToReadingNumber,
                RecordCount=withoutBillData.Count(),
                ReportDate=DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto>(ReportLiterals.WithoutBill, withoutBillHeader, withoutBillData);
            return result;
        }

        private string GetWithoutBillQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId IN @ZoneIds" : string.Empty;

            return @$"Select 
						c.CustomerNumber as CustomerNumber,
						c.FirstName +' '+c.SureName as FullName,
						c.WaterDiameterTitle as MeterDiameterTitle,
						c.UsageTitle2 as UsageSellTitle,
						c.Address as Address,
						c.ZoneTitle as ZoneTitle
					From [CustomerWarehouse].dbo.Clients c
					LEFt JOIN [CustomerWarehouse].dbo.Bills b
					on c.BillId =b.BillId collate SQL_Latin1_General_CP1_CI_AS
					where 
						 b.Id IS NULL
						 AND (@FromDate IS NULL or
						   	  @ToDate IS NULL or 
						   	  b.RegisterDay BETWEEN @FromDate and @ToDate)
						 AND (@FromReadingNumber IS NULL or
						     @ToReadingNumber IS NULL or 
    						 TRY_CAST(c.ReadingNumber AS INT) BETWEEN @FromReadingNumber and @ToReadingNumber)
                         {zoneQuery}";
        }
    }
}
