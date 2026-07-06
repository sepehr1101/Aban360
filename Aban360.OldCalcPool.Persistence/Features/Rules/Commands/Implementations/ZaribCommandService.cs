using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Dapper;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations
{
    public sealed class ZaribCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public ZaribCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }


        public async Task Insert(ZaribCreateDto input)
        {
            string command = GetZaribCreateQuery();
            int effectedRecord = await _connection.ExecuteAsync(command, input, _transaction);
            if (effectedRecord <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidInsertZarib);
            }
        }
        public async Task Update(ZaribUpdateDto input)
        {
            string command = GetZaribUpdateQuery();
            int effectedRecord = await _connection.ExecuteAsync(command, input, _transaction);
            if (effectedRecord <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidUpdateZarib);
            }
        }

        private string GetZaribCreateQuery()
        {
            return @"INSERT INTO [OldCalc].dbo.Zarib 
                    (
                        town,zone1,zone2 ,
                        zarib_baha, date1, date2,
                        zb, zb1, zb2, zb3, zb4, 
                        zb5, zb6, zb7, zb8, zb_r
                    ) 
                    Values
                    (
                        @town,@ZoneTitle1,@ZoneTitle2,
                        @zarib_baha, @date1, @date2, 
                        @zb, @zb1, @zb2, @zb3, @zb4,
                        @zb5, @zb6, @zb7, @zb8, @zb_r
                    )";
        }
        private string GetZaribUpdateQuery()
        {
            return @"UPDATE [OldCalc].dbo.zarib 
                    SET 
                    	town = @town, 
                        zone1 = @zoneTitle1,       
                        zone2 = @zoneTitle2,       
                    	zarib_baha = @zarib_baha,
                    	date1 = @date1, 
                    	date2 = @date2, 
                    	zb = @zb, 
                    	zb1 = @zb1, 
                    	zb2 = @zb2, 
                    	zb3 = @zb3,
                    	zb4 = @zb4,
                    	zb5 = @zb5,
                    	zb6 = @zb6,
                    	zb7 = @zb7,
                    	zb8 = @zb8,
                    	zb_r = @zb_r 
                    WHERE Id = @Id;";
        }
    }
}
