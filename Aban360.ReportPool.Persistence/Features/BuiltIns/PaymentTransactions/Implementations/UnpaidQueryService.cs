using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class UnpaidQueryService : AbstractBaseConnection, IUnpaidQueryService
    {
        public UnpaidQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<UnpaidHeaderOutputDto, UnpaidDataOutputDto>> GetInfo(UnpaidInputDto input)
        {
            string unpaids = GetUnpaidQuery(input.ZoneIds?.Any()==true);
            var @params = new
            {
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                FromDate=input.FromDateJalali,
                ToDate=input.ToDateJalali,
                FromReadingNumber=input.FromReadingNumber,
                ToReadingNumber=input.ToReadingNumber,
                ZoneIds=input.ZoneIds,
            };
            IEnumerable<UnpaidDataOutputDto> unpaidData = await _sqlReportConnection.QueryAsync<UnpaidDataOutputDto>(unpaids,@params);
            UnpaidHeaderOutputDto unpaidHeader = new UnpaidHeaderOutputDto()
            { };

            var result = new ReportOutput<UnpaidHeaderOutputDto, UnpaidDataOutputDto>(ReportLiterals.Unpaid, unpaidHeader, unpaidData);
            return result;
        }

        private string GetUnpaidQuery(bool hasZone)
        {
            string zoneQuery =hasZone? "AND b.ZoneId IN @ZoneIds":string.Empty;

            return @$"SELECT 
                    	MAX(b.CustomerNumber) AS CustomerNumber,
                    	MAX(b.ReadingNumber) AS ReadingNumber,
                    	--b.FirstName +' '+b.SureName AS FullName--todo: firstName Surename not in database
                    	 MAX(b.WaterDiameterTitle) AS MeterDiameterTitle,
                    	 MAX(b.UsageTitle) AS UsageSellTitle,
                    	 MAX(b.PreDebt) AS DebtAmount,--todo: yes?
                    	--b.Address --todo: not in datebase
                    	COUNT(b.BillId) AS PeriodCount,
                    	MAX(b.BillId) AS BillId
                    
                    FROM [CustomerWarehouse].dbo.bills as b
                    LEFT JOIN [CustomerWarehouse].dbo.payments as p ON p.BillTableId = b.id
                    WHERE 
                    p.id IS NULL
                    AND (@FromAmount is null or
                    	 @ToAmount is null or 
                    	 b.Payable BETWEEN @FromAmount and @ToAmount)
                    AND (@FromReadingNumber is null or
                    	 @ToReadingNumber is null or 
                    	 TRY_CAST(b.ReadingNumber AS INT) BETWEEN @FromReadingNumber and @ToReadingNumber)
                    {zoneQuery}
                    group by b.BillId";

            //todo: FromDate ToDate Compared With?????
            //todo: firstName SureName Address not in DataBase
        }
    }
}
