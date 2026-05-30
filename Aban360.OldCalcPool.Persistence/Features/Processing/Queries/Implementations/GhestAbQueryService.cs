using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Implementations
{
    internal sealed class GhestAbQueryService : AbstractBaseConnection, IGhestAbQueryService
    {
        public GhestAbQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<BillInstallmentOutputDto>> Get(ZoneIdAndCustomerNumber inputDto)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetCustomerInstallmentsQuery(dbName);
            IEnumerable<BillInstallmentOutputDto> result = await _sqlReportConnection.QueryAsync<BillInstallmentOutputDto>(query, inputDto);
            return result;
        }
        public async Task<IEnumerable<BillInstallmentOutputDto>> Get(ZoneIdAndCustomerNumber inputDto, string dateJalali)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetByDateJalaliQuery(dbName);
            IEnumerable<BillInstallmentOutputDto> result = await _sqlReportConnection.QueryAsync<BillInstallmentOutputDto>(query, new { inputDto.CustomerNumber, dateJalali });
            return result;

        }
        public async Task<IEnumerable<BillInstallmentOutputDto>> GetLatestBatch(ZoneIdAndCustomerNumber inputDto)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetLatestBatchByCustomerNumberQuery(dbName);
            IEnumerable<BillInstallmentOutputDto> result = await _sqlReportConnection.QueryAsync<BillInstallmentOutputDto>(query, inputDto);
            return result;

        }
        public async Task<BillInstallmentOutputDto> Get(ZoneIdAndCustomerNumber inputDto, int id)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetByIdQuery(dbName);
            BillInstallmentOutputDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<BillInstallmentOutputDto>(query, new { id, inputDto.ZoneId, inputDto.CustomerNumber });
            if (result == null)
            {
                throw new InvalidInstallmentException(ExceptionLiterals.NotFountBillInstallmentId);
            }
            return result;
        }


        private string GetCustomerInstallmentsQuery(string dbName)
        {
            return $@"With Cte As(
                    	Select 
                    		*,
                    		Rn=ROW_NUMBER() OVER(Partition by date_bed Order By mohlat Desc)
                    	From [{dbName}].dbo.ghest_ab
                    	Where radif=@CustomerNumber
                    )
                    Select 
                    	g.ID,
                    	g.town ZoneId,
                    	g.radif CustomerNumber,
                    	TRIM(g.eshtrak) ReadingNumber,
                    	g.barge Barge,
                    	g.date_bed RegisterDateJalali,
                    	g.mohlat DeadLineDateJalali,
                    	g.pard Payable,
                    	g.cod_enshab UsageId,
                    	t41.C1 UsageTitle,
                    	g.enshab MeterDiamterId,
                    	t5.C2 MeterDiameterTitle,
						g.serial QueueNumber,
	                	g.operator InsertedBy
                   From [{dbName}].dbo.ghest_ab g
                    Join Cte cg
						On g.date_bed=cg.date_bed
					Join [Db70].dbo.T41 t41
                    	ON g.cod_enshab=t41.C0
                    Join [Db70].dbo.T5 t5
                    	ON g.enshab=t5.C0
                    Where 
                    	g.town=@ZoneId AND
                    	g.radif=@CustomerNumber AND
						CustomerWarehouse.dbo.PersianToMiladi(cg.mohlat)>=CAST( DATEFROMPARTS( YEAR(GETDATE()), MONTH(GETDATE()), DAY(GETDATE() )) AS datetime)AND
						cg.Rn=1
					Order By g.date_bed Asc, g.mohlat Asc";
        }
        private string GetByDateJalaliQuery(string dbName)
        {
            return $@"Select 
	                	g.ID,
	                	g.town ZoneId,
	                	g.radif CustomerNumber,
	                	TRIM(g.eshtrak) ReadingNumber,
	                	g.barge Barge,
	                	g.date_bed RegisterDateJalali,
	                	g.mohlat DeadLineDateJalali,
	                	g.pard Payable,
	                	g.cod_enshab UsageId,
	                	t41.C1 UsageTitle,
	                	g.enshab MeterDiamterId,
	                	t5.C2 MeterDiameterTitle,
	                	g.serial QueueNumber,
	                	g.operator InsertedBy
	                From [{dbName}].dbo.ghest_ab g
	                Join [Db70].dbo.T41 t41
	                	ON g.cod_enshab=t41.C0
	                Join [Db70].dbo.T5 t5
	                	ON g.enshab=t5.C0
	                where radif=@CustomerNumber  AND date_bed=@dateJalali
	                order by date_bed desc";
        }
        private string GetLatestBatchByCustomerNumberQuery(string dbName)
        {
            return $@"With Cte As(
                    	Select Top 1*
                    	From [{dbName}].dbo.ghest_ab
                    	Where radif=@CustomerNumber
                    	Order by date_bed Desc
                    )
                    Select 
	                	g.ID,
	                	g.town ZoneId,
	                	g.radif CustomerNumber,
	                	TRIM(g.eshtrak) ReadingNumber,
	                	g.barge Barge,
	                	g.date_bed RegisterDateJalali,
	                	g.mohlat DeadLineDateJalali,
	                	g.pard Payable,
	                	g.cod_enshab UsageId,
	                	t41.C1 UsageTitle,
	                	g.enshab MeterDiamterId,
	                	t5.C2 MeterDiameterTitle,
	                	g.serial QueueNumber,
	                	g.operator InsertedBy
	                From [{dbName}].dbo.ghest_ab g
					Join Cte c
						ON g.date_bed=c.date_bed
	                Join [Db70].dbo.T41 t41
	                	ON g.cod_enshab=t41.C0
	                Join [Db70].dbo.T5 t5
	                	ON g.enshab=t5.C0
	                where g.radif=@CustomerNumber  
	                order by g.mohlat";
        }
        private string GetByIdQuery(string dbName)
        {
            return $@"Select 
                    	g.ID,
                    	g.town ZoneId,
                    	g.radif CustomerNumber,
                    	TRIM(g.eshtrak) ReadingNumber,
                    	g.barge Barge,
                    	g.date_bed RegisterDateJalali,
                    	g.mohlat DeadLineDateJalali,
                    	g.pard Payable,
                    	g.cod_enshab UsageId,
                    	t41.C1 UsageTitle,
                    	g.enshab MeterDiamterId,
                    	t5.C2 MeterDiameterTitle,
						g.serial QueueNumber,
	                	g.operator InsertedBy
                   From [{dbName}].dbo.ghest_ab g
                   Join [Db70].dbo.T41 t41
                     ON g.cod_enshab=t41.C0
                   Join [Db70].dbo.T5 t5
                   	 ON g.enshab=t5.C0
                   Where 
                   	g.ID=@id AND
                   	g.radif=@customerNumber AND
                   	g.town=@zoneId";
        }
    }
}
