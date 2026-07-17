using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Implementations
{
    public sealed class SmsTypeService : AbstractBaseConnection, ISmsTypeService
    {
        public SmsTypeService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Insert(string title)
        {
            string command = GetInsertCommand();
            int effectedRecord = await _sqlReportConnection.ExecuteAsync(command, new { title });
            if (effectedRecord <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidInsertSmsType);
            }
        }
        public async Task<NumericDictionary> Get(int id)
        {
            string query = GetByIdQuery();
            NumericDictionary? SmsType = await _sqlReportConnection.QueryFirstOrDefaultAsync<NumericDictionary>(query, new { id });
            if (SmsType is null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidId);
            }
            return SmsType;
        }
        public async Task<IEnumerable<NumericDictionary>> Get()
        {
            string query = GetQuery();
            IEnumerable<NumericDictionary> result = await _sqlReportConnection.QueryAsync<NumericDictionary>(query);
            return result;
        }
        private string GetInsertCommand()
        {
            return @"INSERT INTO Atlas.dbo.SmsType(Title)
                    VALUES(@Title)";
        }
        private string GetQuery()
        {
            return @"SELECT
                        Id,
                        Title
                    FROM Atlas.dbo.SmsType ";
        }
        private string GetByIdQuery()
        {
            return @"SELECT
                        Id,
                        Title
                    FROM Atlas.dbo.SmsType
                    WHERE Id = @Id ";
        }
    }
}
