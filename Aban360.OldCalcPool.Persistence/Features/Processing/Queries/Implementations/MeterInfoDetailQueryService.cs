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
        { }

        public async Task<MeterInfoOutputDto> GetInfo(CustomerInfoInputDto input)
        {
            string meterInfoQueryString = GetMeterInfoDataQuery(input.ZoneId);
            var @params = new
            {
                zoneId = input.ZoneId,
                radif = input.Radif
            };
            MeterInfoOutputDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<MeterInfoOutputDto>(meterInfoQueryString, @params);

            return result;
        }

        private string GetMeterInfoDataQuery(int zoneId)
        {
            return @$"Select
                    	IIF(c.pri_date > c.taviz_date, c.pri_no,c.taviz_no) as PreviousNumber,
                    	IIF(c.pri_date > c.taviz_date, c.pri_date,c.taviz_date) as PreviousDateJalali,
                    	c.cod_vas as BranchType
                    From [{zoneId}].dbo.contor c
                    Where 
                    	c.town=@zoneId AND
                    	c.radif=@radif AND
                    	c.cod_vas NOT IN (4,7,8) AND
                    	c.mohasbat=2 ";
        }
    }
}
