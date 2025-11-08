using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Implementations
{
    internal sealed class MeterInfoDetailQueryService : AbstractBaseConnection, IMeterInfoDetailQueryService
    {
        public MeterInfoDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<MeterInfoOutputDto> GetInfo(CustomerInfoInputDto input)
        {
            string dbName = GetDbName(input.ZoneId);
            string meterInfoQueryString = GetMeterInfoDataQuery(dbName, input.ZoneId);
            var @params = new
            {
                dbName,
                zoneId = input.ZoneId,
                radif = input.Radif
            };
            MeterInfoOutputDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<MeterInfoOutputDto>(meterInfoQueryString, @params);

            return result;
        }

        private string GetMeterInfoDataQuery(string dbName, int zoneId)
        {
            return @$"
                    Select top 1
                        c.pri_no as PreviousNumber,
                        c.pri_date as PreviousDateJalali,
                        c.today_no CurrentNumber,
                        c.today_date CurrentDateJalali
                    From [{dbName}].dbo.bed_bes c
                    Where 
                        c.town=@zoneId AND
                        c.radif=@radif AND
                        c.cod_vas NOT IN (4,7,8)
                    ORDER BY c.date_bed desc";
        }
    }
}
