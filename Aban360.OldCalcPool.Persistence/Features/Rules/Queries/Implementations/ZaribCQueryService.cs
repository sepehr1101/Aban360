using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Implementations
{
    internal sealed class ZaribCQueryService : AbstractBaseConnection, IZaribCQueryService
    {
        public ZaribCQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ZaribCQueryDto> GetZaribC(string @from, string @to)
        {
            string query = GetQueryByFromTo();
            var @params = new
            {
                fromDate = @from,
                toDate = @to
            };
            ZaribCQueryDto zaribCQueryDto = await _sqlReportConnection.QueryFirstAsync(query, @params);
            return zaribCQueryDto;
        }
        private string GetQueryByFromTo()
        {
            return
                @"SELECT 
	                Id,
	                FromDateJalali,
	                ToDateJalali,
	                C
                FROM [OldCalc].dbo.Zarib_C
                WHERE 
	                FromDateJalali=@fromDate AND ToDateJalali=@toDate AND
	                IsDeleted=0";
        }
    }
}
