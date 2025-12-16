using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Exceptions;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Implementations
{
    internal sealed class SQueryService : AbstractBaseConnection, ISQueryService
    {
        public SQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<SGetDto>> Get(string @from, string @to)
        {
            string query = GetQueryByFromTo();
            var @params = new
            {
                fromDate = @from,
                toDate = @to
            };
            IEnumerable<SGetDto> result = await _sqlReportConnection.QueryAsync<SGetDto>(query, @params);
            return result;
        }
        public async Task<SGetDto> Get(string @from, string @to,int zoneId)
        {
            string query = GetQueryByFromToZoneId();
            var @params = new
            {
                fromDate = @from,
                toDate = @to,
                zoneId = zoneId 
            };
            SGetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<SGetDto>(query, @params);
            return result;
        }
        public async Task<IEnumerable<SGetDto>> Get()
        {
            string query = GetAllQuery();
            IEnumerable<SGetDto> result = await _sqlReportConnection.QueryAsync<SGetDto>(query);

            return result;
        }
        public async Task<SGetDto> Get(int id)
        {
            string query = GetSingleQuery();
            SGetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<SGetDto>(query, new { id });
            if (result == null)
            {
                throw new InvalidIdException();
            }

            return result;
        }
        public async Task<SGetDto> Get(string currentDateJalali)
        {
            string query = GetQueryByDate();
            SGetDto result = await _sqlReportConnection.QueryFirstAsync<SGetDto>(query, new { currentDateJalali });
            return result;
        }

        private string GetQueryByFromTo()
        {
            return @"Select 
                    	Id,
                    	town as ZoneId,
                    	olgo,
                    	FromDate as FromDateJalali,
                    	ToDate as ToDateJalali
                    From OldCalc.dbo.S
                    Where 
                    	FromDate>=@fromDate And
                    	ToDate<=@toDate ";
        }
        private string GetQueryByFromToZoneId()
        {
            return @"Select 
                    	Id,
                    	town as ZoneId,
                    	olgo,
                    	FromDate as FromDateJalali,
                    	ToDate as ToDateJalali
                    From OldCalc.dbo.S
                    Where 
                    	FromDate>=@fromDate And
                    	ToDate<=@toDate And
                        Town=@zoneId
					Order By FromDate Desc";
        }
        private string GetAllQuery()
        {
            return @"Select 
                    	Id,
                    	town as ZoneId,
                    	olgo,
                    	FromDate as FromDateJalali,
                    	ToDate as ToDateJalali
                    From OldCalc.dbo.S";
        }
        private string GetSingleQuery()
        {
            return @"Select 
                    	Id,
                    	town as ZoneId,
                    	olgo,
                    	FromDate as FromDateJalali,
                    	ToDate as ToDateJalali
                    From OldCalc.dbo.S
                    Where Id=@Id";
        }
        private string GetQueryByDate()
        {
            return @"Select 
                    	Id,
                    	town as ZoneId,
                    	olgo,
                    	FromDate as FromDateJalali,
                    	ToDate as ToDateJalali
                    From OldCalc.dbo.S
                    Where 
                    	@currentDateJalali BETWEEN FromDate AND ToDate";
        }
    }
}
