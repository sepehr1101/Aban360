using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Dapper;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations
{
    public sealed class SCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public SCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Update(SUpdateDto input)
        {
            string SUpdateQueryString = GetSUpdateQuery();
            var @params = new
            {
                id = input.Id,
                town = input.ZoneId,
                input.Olgo,
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali
            };
            int effectedRecord = await _connection.ExecuteAsync(SUpdateQueryString, @params, _transaction);
            if (effectedRecord <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidUpdateS);
            }
        }
        public async Task Insert(SCreateDto input)
        {
            int latestId = await GetLatestId();

            string SCreateQueryString = GetSCreateQuery();
            var @params = new
            {
                id = latestId + 1,
                town = input.ZoneId,
                input.Olgo,
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali
            };
            int effectedRecord = await _connection.ExecuteAsync(SCreateQueryString, @params, _transaction);
            if (effectedRecord <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidInsertS);
            }
        }
        public async Task Delete(int id)
        {
            string SDeleteQueryString = GetSDeleteQuery();
            int effectedRecord = await _connection.ExecuteAsync(SDeleteQueryString, new { id }, _transaction);
            if (effectedRecord <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidRemoveS);
            }
        }
        private async Task<int> GetLatestId()
        {
            string query = GetLatestIdQuery();
            int latestId = await _connection.QueryFirstOrDefaultAsync<int>(query, null, _transaction);

            return latestId;
        }

        private string GetSUpdateQuery()
        {
            return @"Update OldCalc.dbo.S
                    Set town=@town , olgo=@olgo , FromDate=@FromDate , ToDate=@FromDate
                    Where Id=@Id";
        }
        private string GetSCreateQuery()
        {
            return @"Insert Into OldCalc.dbo.S(town,olgo,FromDate,ToDate)
                    Values(@town,@olgo,@FromDate,@ToDate)
";
        }
        private string GetSDeleteQuery()
        {
            return @"Delete from OldCalc.dbo.s
                    Where Id=@Id";
        }
        private string GetLatestIdQuery()
        {
            return @"Select top 1 Id
                    From OldCalc.dbo.S 
                    Order By Id Desc";
        }
    }
}
