using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
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
        public async Task<IEnumerable<BillsCanRemovedOutputDto>> GetToRemove(RemovedBillSearchDto input)
        {
            //string dbName = GetDbName(input.ZoneId);
            string dbName = "Atlas";
            string query = GetBedBesListToRemove(dbName);

            IEnumerable<BillsCanRemovedOutputDto> result = await _sqlReportConnection.QueryAsync<BillsCanRemovedOutputDto>(query, input);
            if (result is null || !result.Any())
            {
                throw new RemovedBillException(ExceptionLiterals.NotFoundBillsToRemoved);
            }
            return result;
        }
        public async Task<RemoveBillInputDto> GetToRemove(int id)
        {
            //string dbName = GetDbName(input.ZoneId);
            string dbName = "Atlas";
            string query = GetBedBesToRemove(dbName);
        public async Task<IEnumerable<BillsCanRemovedOutputDto>> GetToReturned(ReturnedBillSearchDto input)
        {
            //string dbName = GetDbName(input.ZoneId);
            string dbName = "Atlas";
            string query = GetAllBedBesToReturned(dbName);

            RemoveBillInputDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<RemoveBillInputDto>(query, new { Id = id });
            if (result is null || result.Id <= 0)
            {
                throw new RemovedBillException(ExceptionLiterals.InvalidId);
            }
            IEnumerable<BillsCanRemovedOutputDto> result=await _sqlReportConnection.QueryAsync<BillsCanRemovedOutputDto>(query,input);
            if (result is null || !result.Any())
            {
                throw new ReturnedBillException(ExceptionLiterals.NotFoundBillsToReturned);
            }
            return result;
        }
        public async Task<IEnumerable<BillsCanRemovedOutputDto>> Get(SearchBillToReturnedDto input)
        {
            //string dbName = GetDbName(input.ZoneId);
            string dbName = "Atlas";
            string query = GetAllBedBesToReturned(dbName);

            IEnumerable<BillsCanRemovedOutputDto> result=await _sqlReportConnection.QueryAsync<BillsCanRemovedOutputDto>(query,input);
            if (result is null || !result.Any())
            {
                throw new ReturnedBillException(ExceptionLiterals.NotFoundBillsToReturned);
            }
            return result;
        }

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
        private string GetBedBesListToRemove(string dbName)
        {
            return @$"SELECT	
                    	b.id,
                    	b.town as ZoneId,
                    	b.radif as CustomerNumber,
                    	b.pri_no as PreviousNumber, 
                    	b.today_no as CurrentNumber,
                    	b.pri_date as PrviousDateJalali,
                    	b.today_date as CurrentDateJalali,
                    	b.date_bed as RegisterDateJalali,
                    	b.masraf as Consumption,
                    	b.rate as MonthlyConsumption,
                    	b.pard as Pardakht,
                    	b.jam as Jam, 
                    	b.baha as Baha,
                    	b.kasr_ha as Discount,
                    	b.mamor as AgentCode,
                        b.barge as Barge,
                        b.sh_pard1 as PaymentId,
                    	b.sh_ghabs1 as BillId,
                    	t41.C1 as UsageTitle,
                    	t7.C1 as BranchTypeTitle,
                    	b.fix_mas as ContractualCapacity,
                    	b.Khali_s as EmptyUnit,
                    	b.tedad_mas as DomesticUnit,
                    	b.tedad_tej as CommercialUnit,
                    	b.tedad_vahd as OtherUnit,
                    	b.ted_khane as HouseholdNumber
                    FROM [{dbName}].dbo.bed_bes b
                    JOIN [{dbName}].dbo.variab v
                    	ON b.date_bed collate Persian_100_CI_AI>=v.date_check
                    JOIN [Db70].dbo.T41 t41 
                    	ON b.cod_enshab=t41.C0
                    JOIN [Db70].dbo.T7 t7 
                    	ON b.noe_va=t7.C0
                    WHERE 
                    	b.date_bed>=@ComparisonDateJalali AND
                    	b.town=@ZoneId AND
                    	b.radif=@CustomerNumber";
        }
        private string GetBedBesToRemove(string dbName)
        {
            return @$"SELECT	
                    	b.id,
                    	b.town as ZoneId,
                    	b.radif as CustomerNumber,
                        b.barge as Barge,
                    	b.pri_no as PreviousNumber, 
                    	b.today_no as CurrentNumber,
                    	b.pri_date as PrviousDateJalali,
                    	b.today_date as CurrentDateJalali,
                    	b.date_bed as RegisterDateJalali,
                    	b.masraf as Consumption,
                        b.sh_pard1 as PaymentId,
						b.ab_baha as AbBahaAmount,
						b.fas_baha as FazelabAmount,
                    	b.baha as Baha,
                    	b.sh_ghabs1 as BillId
                    FROM [{dbName}].dbo.bed_bes b
                    WHERE 
                    	b.id=@id";
        }
    }
}
