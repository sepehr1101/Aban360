using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Exceptions;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;

namespace Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Implementations
{
    internal sealed class CounterStateQueryService : AbstractBaseConnection, ICounterStateQueryService
    {
        public CounterStateQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<CounterStateCodeDto> Get(int id, bool hasException)
        {
            string query = GetSingleQuery();
            CounterStateCodeDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<CounterStateCodeDto>(query, new { id });
            if (result is null && hasException)
            {
                throw new InvalidIdException();
            }

            return result;
        }

        public async Task<IEnumerable<CounterStateCodeDto>> Get(bool isReadable)
        {
            string query = GetAllQuery(isReadable);
            IEnumerable<CounterStateCodeDto> result = await _sqlReportConnection.QueryAsync<CounterStateCodeDto>(query, null);

            return result;
        }

        private string GetSingleQuery()
        {
            return @"Select 
                    	MoshtarakinId as Id,
                    	Title	
                    From [Db70].dbo.CounterVaziat
                    Where MoshtarakinId=@id";
        }
        private string GetAllQuery(bool isReadable)
        {
            string condition = isReadable ? "And IsReadable=1" : string.Empty;
            return @$"Select 
                    	MoshtarakinId as Id,
                    	Title	
                    From [Db70].dbo.CounterVaziat 
                    WHERE 
                        MoshtarakinId<9 {condition}";
        }
    }
}
