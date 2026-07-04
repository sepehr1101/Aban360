using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Implementations
{
    internal sealed class AbonmanQueryService : AbstractBaseConnection, IAbonmanQueryService
    {
        const string _minDate = "1402/12/29";
        public AbonmanQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<AbonmanGetDto>> Get()
        {
            string AbonmanGetQueryString = GetQuery();
            IEnumerable<AbonmanGetDto> result = await _sqlReportConnection.QueryAsync<AbonmanGetDto>(AbonmanGetQueryString);

            return result;
        }
        public async Task<AbonmanGetDto> Get(int id)
        {
            string AbonmanGetQueryString = GetByIdQuery();
            AbonmanGetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<AbonmanGetDto>(AbonmanGetQueryString, new { id });
            if (result == null)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.NotFoundData);
            }
            return result;
        }

        private string GetQuery()
        {
            return @$"Select
                        Id,
                		date1 AS Date1,
                		date2 AS Date2,
                		vaj AS Vaj,
                		cod AS Code,
                		[desc] AS [Desc]
                	From [OldCalc].dbo.Abonman 
                    Where date1>='{_minDate}'";
        }
        private string GetByIdQuery()
        {
            return @$"Select
                        Id,
                		date1 AS Date1,
                		date2 AS Date2,
                		vaj AS Vaj,
                		cod AS Code,
                		[desc] AS [Desc]
                	From [OldCalc].dbo.Abonman 
                    Where 
                        date1>='{_minDate}' AND
                        Id=@id";
        }
    }
}