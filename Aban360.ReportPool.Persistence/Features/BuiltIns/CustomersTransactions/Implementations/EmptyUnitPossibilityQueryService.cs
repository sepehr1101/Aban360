using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class EmptyUnitPossibilityQueryService : AbstractBaseConnection, IEmptyUnitPossibilityQueryService
    {
        public EmptyUnitPossibilityQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<EmptyUnitPossibilityHeaderOutputDto, EmptyUnitPossibilityDataOutputDto>> GetInfo(EmptyUnitPossibilityInputDto input)
        {
            string query = GetQuery();
            IEnumerable<EmptyUnitPossibilityDataOutputDto> data = await _sqlReportConnection.QueryAsync<EmptyUnitPossibilityDataOutputDto>(query, input);
            EmptyUnitPossibilityHeaderOutputDto header = new EmptyUnitPossibilityHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToateJalali = input.ToDateJalali,
                CustomerCount = data.Count(),
                RecordCount = data.Count(),
                ReportDateJalali = DateTime.Now.ToShortPersianDateString()
            };
            ReportOutput<EmptyUnitPossibilityHeaderOutputDto, EmptyUnitPossibilityDataOutputDto> result = new(ReportLiterals.EmptyUnitPossibility, header, data);

            return result;
        }

        private string GetQuery()
        {
            return @"Select 
                    	b.ZoneId ,
                    	b.CustomerNumber,
                    	b.BillId,
                    	b.ConsumptionAverage,
                    	b.CommercialCount as CommertialUnit,
                    	b.DomesticCount as DomesticUnit,
                    	b.OtherCount as OtherUnit,
                    	b.EmptyCount as EmptyUnit,
                    	b.UsageTitle,
                    	b.ZoneTitle
                    From CustomerWarehouse.dbo.Bills b
                    Left Join CustomerWarehouse.dbo.Clients c		 
                    	ON b.ZoneId=c.ZoneId AND b.CustomerNumber=c.CustomerNumber
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	b.ZoneId IN @ZoneIds AND
                    	b.UsageId IN (1,3) AND
                    	b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali AND
                    	b.EmptyCount=0 AND
                    	b.DomesticCount>1 AND
                    	((( 9.33 - b.ConsumptionAverage ) / 9.33 )*( b.CommercialCount + b.DomesticCount + b.OtherCount )) >= 1";
        }
    }
}
