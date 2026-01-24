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
            ZaribCQueryDto zaribCQueryDto = await _sqlReportConnection.QueryFirstAsync<ZaribCQueryDto>(query, @params);
            return zaribCQueryDto;
        }
        public async Task<ZaribCQueryDto> GetLatestZaribC(string @from, string @to)
        {
            string query = GetLatestQuery();
            ZaribCQueryDto zaribCQueryDto = await _sqlReportConnection.QueryFirstOrDefaultAsync<ZaribCQueryDto>(query, null);
            return zaribCQueryDto;
        }
        public async Task<IEnumerable<ZaribCQueryDto>> GetZaribC()
        {
            string query = GetAllQuery();
            IEnumerable<ZaribCQueryDto> zaribCQueryDto = await _sqlReportConnection.QueryAsync<ZaribCQueryDto>(query);

            return zaribCQueryDto;
        }
        public async Task<ZaribCQueryDto> GetZaribC(string currentDateJalali)
        {
            string query = GetQueryByDate();
            ZaribCQueryDto zaribCQueryDto = await _sqlReportConnection.QueryFirstAsync<ZaribCQueryDto>(query, new { currentDateJalali });
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
        private string GetLatestQuery()
        {
            return
                @"SELECT Top 1
                    Id,
                    FromDateJalali,
                    ToDateJalali,
                    C
                FROM [OldCalc].dbo.Zarib_C
                WHERE 
                    IsDeleted=0
                Order by ToDateJalali DESC";
        }
        private string GetAllQuery()
        {
            return @"SELECT 
	                   Id,
	                   FromDateJalali,
	                   ToDateJalali,
	                   C
                    FROM [OldCalc].dbo.Zarib_C";
        }
        private string GetQueryByDate()
        {
            return @"SELECT 
	                Id,
	                FromDateJalali,
	                ToDateJalali,
	                C
                FROM [OldCalc].dbo.Zarib_C
                WHERE 
	               @currentDateJalali BETWEEN FromDateJalali AND ToDateJalali AND
	                IsDeleted=0";
        }
    }
}
