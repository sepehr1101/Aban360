using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                FromDate=input.FromDateJalali,
                ToDate=input.ToDateJalali,
                FromReadingNumber=input.FromReadingNumber,
                ToReadingNumber=input.ToReadingNumber,
                ZoneIds=input.ZoneIds,
            };

            IEnumerable<WithoutBillDataOutputDto> withoutBillData = await _sqlReportConnection.QueryAsync<WithoutBillDataOutputDto>(withoutBill,@params);
            WithoutBillHeaderOutputDto withoutBillHeader = new WithoutBillHeaderOutputDto()
            {
                FromDateJalali=input.FromDateJalali,
                ToDateJalali=input.ToDateJalali,
                FromReadingNumber=input.FromReadingNumber,
                ToReadingNumber=input.ToReadingNumber,
                RecordCount= (withoutBillData is not null && withoutBillData.Any()) ? withoutBillData.Count() : 0,
                ReportDateJalali =DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto>(ReportLiterals.WithoutBill, withoutBillHeader, withoutBillData);
            return result;
        }

        private string GetWithoutBillQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND c.ZoneId IN @ZoneIds" : string.Empty;

            return @$"Select 
						c.CustomerNumber as CustomerNumber,
                        c.ReadingNumber,
						TRIM(c.FirstName) +' '+TRIM(c.SureName) as FullName,
						c.WaterDiameterTitle as MeterDiameterTitle,
						c.UsageTitle2 as UsageSellTitle,
						TRIM(c.Address) as Address,
						c.ZoneTitle as ZoneTitle
					From [CustomerWarehouse].dbo.Clients c
					LEFt JOIN [CustomerWarehouse].dbo.Bills b
					on c.ZoneId=b.ZoneId AND c.CustomerNumber=b.CustomerNumber
					where 
						 b.Id IS NULL
						 AND (@FromDate IS NULL or
						   	  @ToDate IS NULL or 
						   	  c.WaterInstallDate BETWEEN @FromDate and @ToDate)
						 AND (@FromReadingNumber IS NULL or
						     @ToReadingNumber IS NULL or 
    						 c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber)
                         {zoneQuery}";
        }
    }
}