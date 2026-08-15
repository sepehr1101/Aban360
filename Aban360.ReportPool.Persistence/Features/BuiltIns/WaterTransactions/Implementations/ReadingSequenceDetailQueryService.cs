using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class ReadingSequenceDetailQueryService : AbstractBaseConnection, IReadingSequenceDetailQueryService
    {
        public ReadingSequenceDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ReadingSequenceHeaderOutputDto, ReadingSequenceDataOutputDto>> GetInfo(ReadingSequenceInputDto input)
        {
            string dbName = GetDbName(input.ZoneId);
            string query = GetQuery(dbName);

            IEnumerable<ReadingSequenceDataOutputDto> data = await _sqlReportConnection.QueryAsync<ReadingSequenceDataOutputDto>(query, input);
            ReadingSequenceHeaderOutputDto header = new ReadingSequenceHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = data?.Count() ?? 0,
                CustomerCount = data?.DistinctBy(x => x.BillId)?.Count() ?? 0,
                Title = ReportLiterals.ReadingSequenceDetail,
            };

            var result = new ReportOutput<ReadingSequenceHeaderOutputDto, ReadingSequenceDataOutputDto>(ReportLiterals.ReadingSequenceDetail, header, data);
            return result;
        }

        private string GetQuery(string dbName)
        {
            return @$";WITH BillsInfo AS(
                    	Select	
                    		Id,
                    		radif CustomerNumber,
                    		sh_ghabs1 BillId,
                    		eshtrak ReadingNumber,
                    		pri_date PreviousDateJalali, 
                    		today_date CurrentDateJalali,
                    		LAG(today_date) OVER(PARTITION BY radif ORDER BY date_bed asc) as PreviousCurrentDateJalali,
                    		pri_no PreviousNumber,
                    		today_no CurrentNumber,
                    		LAG(today_no) OVER(PARTITION BY radif ORDER BY date_bed asc) as PreviousCurrentNumber,
                    		cod_vas CounterStateCode,
                    		LAG(cod_vas) OVER(PARTITION BY radif  ORDER BY date_bed asc) as PreviousCounterStateCode
                    	From [{dbName}].dbo.bed_bes
                    	Where 
                    		cod_vas NOT IN(2,4,7,8) AND
                    		date_bed BETWEEN @FromDateJalali AND @ToDateJalali
                    )
                    SELECT
                    	b.id,
                        b.CustomerNumber,
                    	b.BillId,
                    	b.ReadingNumber,
                    	b.PreviousDateJalali,
                        b.CurrentDateJalali,
                    	b.PreviousCurrentDateJalali,
                        b.PreviousNumber,
                        b.CurrentNumber,
                    	b.PreviousCurrentNumber,
                        b.CounterStateCode,
                    	c.Title CounterStateTitle
                    FROM BillsInfo b
                    LEFT JOIN [Db70].dbo.CounterVaziat c
                    	ON b.CounterStateCode=c.MoshtarakinId
                    Where 
                    	b.PreviousCurrentDateJalali <> b.PreviousDateJalali AND
                    	b.PreviousCurrentNumber <> b.PreviousNumber AND
                    	b.PreviousCounterStateCode NOT IN (2,4,7,8)
                    Order By b.BillId,b.CurrentDateJalali Desc ";
        }
    }
}
