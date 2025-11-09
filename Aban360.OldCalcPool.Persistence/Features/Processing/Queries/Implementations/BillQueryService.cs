using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Implementations
{
    internal sealed class BillQueryService : AbstractBaseConnection, IBillQueryService
    {
        public BillQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<BedBesConsumptionOutputDto> Get(string billId)
        {
            string zoneIdQueryString = GetZoneIdQuery();
            ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = await _sqlReportConnection.QueryFirstOrDefaultAsync<ZoneIdAndCustomerNumberOutputDto>(zoneIdQueryString, new { billId });
            if (zoneIdAndCustomerNumber == null)
            {
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound + billId);
            }
            string DataBaseName = GetDbName(zoneIdAndCustomerNumber.ZoneId);
            string customerInfoQueryString = GetBedBesConsumptionDataQuery(DataBaseName);
            BedBesConsumptionOutputDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<BedBesConsumptionOutputDto>(customerInfoQueryString, new { billId });

            return result;
        }
        public async Task<BedBesDataInfoOutptuDto> Get(int id)
        {
            string customerInfoQueryString = GetBedBesByIdQuery();
            BedBesDataInfoOutptuDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<BedBesDataInfoOutptuDto>(customerInfoQueryString, new { Id = id });

            return result;
        }

        private string GetBedBesConsumptionDataQuery(string dataBaseName)
        {
            return @$"Select Top 1 
                        rate as ConsumptionAverage
                    From [{dataBaseName}].dbo.bed_bes 
                    Where 
                    	TRIM(sh_ghabs1)=@billId AND
                    	cod_vas NOT IN (4,7,8)
                    Order By 
                    	today_date DESC,
                    	pri_date DESC";
        }
        private string GetBedBesByIdQuery()
        {
            return @$"Select 
                        sh_ghabs1 as BillId,
	                    pri_date as PreviousDateJalali,
	                    today_date as CurrentDateJalali,
	                    date_bed as DateBed,
	                    cod_vas as CounterStateCode,
	                    serial as BodySerial
                    From [Atlas].dbo.bed_bes 
                    Where 
                    	Id=@Id AND
                    	cod_vas NOT IN (4,7,8)";
        }
        private string GetZoneIdQuery()
        {
            return @"Select c.ZoneId,c.CustomerNumber
                    From [CustomerWarehouse].dbo.Clients c
                    Where 
                    	c.BillId=@billId AND
                    	c.ToDayJalali IS NULL";
        }
    }
}
