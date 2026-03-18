using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations
{
    public sealed class T0CommandService
    {
        private readonly IDbConnection _sqlRonnection;
        private readonly IDbTransaction _transaction;
        public T0CommandService(
            IDbConnection sqlRonnection,
            IDbTransaction transaction)
        {
            _sqlRonnection = sqlRonnection;
            _sqlRonnection.NotNull(nameof(sqlRonnection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task<decimal> GetTrackNumber()
        {
            int trackNumber = await _sqlRonnection.QueryFirstOrDefaultAsync<int>(GetTrackNumberQuery(), null, _transaction);
            int recordCount = await _sqlRonnection.ExecuteAsync(GetIncreaseTrackNumberCommand(), new { Count = 1 }, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateTrackNumber);
            }

            string cookedTrackNumber = GetTrackNumberWithCheckDigit(trackNumber.ToString());
            return Convert.ToDecimal(cookedTrackNumber);
        }
        private string GetTrackNumberWithCheckDigit(string trackNumber)
        {
            Random rand = new Random();
            string checkDigit = rand.Next(0, 10).ToString();
            return (trackNumber + checkDigit);
        }


        private string GetIncreaseTrackNumberCommand()
        {
            return $@"Update AbAndFazelab.dbo.T0
                    Set C1=C1+@Count
                    Where C0=1";
        }
        private string GetTrackNumberQuery()
        {
            return $@"Select C1
                    From AbAndFazelab.dbo.T0
                    Where C0=1";
        }
    }
}
