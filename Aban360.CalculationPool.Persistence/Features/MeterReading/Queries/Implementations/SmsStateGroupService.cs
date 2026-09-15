using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Implementations
{
    public sealed class SmsStateGroupService : AbstractBaseConnection, ISmsStateGroupService
    {
        public SmsStateGroupService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<NumericDictionary> Get(int id)
        {
            string query = GetByIdQuery();
            NumericDictionary? SmsStateGroup = await _sqlReportConnection.QueryFirstOrDefaultAsync<NumericDictionary>(query, new { id });
            if (SmsStateGroup is null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidId);
            }
            return SmsStateGroup;
        }
        public async Task<NumericDictionary?> Get(string title, bool hasException)
        {
            string query = GetByTitleQuery();
            NumericDictionary? smsStateGroup = await _sqlReportConnection.QueryFirstOrDefaultAsync<NumericDictionary>(query, new { title });
            if (smsStateGroup is null && hasException)
            {
                throw new ReadingException(ExceptionLiterals.NotFoundData);
            }
            return smsStateGroup;
        }
     

        public async Task<IEnumerable<NumericDictionary>> Get()
        {
            string query = GetQuery();
            IEnumerable<NumericDictionary> result = await _sqlReportConnection.QueryAsync<NumericDictionary>(query);
            return result;
        }

        private string GetQuery()
        {
            return @"SELECT
                        Id,
                        Title
                    FROM Atlas.dbo.SmsStateGroup ";
        }
        private string GetByIdQuery()
        {
            return @"SELECT
                        Id,
                        Title
                    FROM Atlas.dbo.SmsStateGroup
                    WHERE Id = @Id ";
        }
        private string GetByTitleQuery()
        {
            return @"SELECT
                        Id,
                        Title
                    FROM Atlas.dbo.SmsStateGroup
                    WHERE Title = @Title ";
        }
    }
}
