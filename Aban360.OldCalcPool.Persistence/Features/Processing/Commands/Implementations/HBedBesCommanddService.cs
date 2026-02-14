using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    public sealed class HBedBesCommanddService //: IHBedBesCommanddService
    {
        private readonly SqlConnection _connection;
        private readonly IDbTransaction _transaction;
        public HBedBesCommanddService(
            SqlConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        //public HBedBesCommanddService(IConfiguration configuration)
        //    : base(configuration)
        //{
        //}

        public async Task Insert(RemoveBillDataInputDto input)
        {
            string command = GetInsertCommand(GetDbName(input.ZoneId));
            await _connection.ExecuteAsync(command, input);
        }

        private string GetInsertCommand(string dbName)
        {
            return @$"INSERT INTO [{dbName}].dbo.Hbedbes(
                        TOWN,radif,barge,
                        date_bed,pri_date,today_date,pri_no,today_no,
                        masraf,ab_baha,fas_baha,baha,
                        SH_GHABS1,SH_PARD1,date,operator)
                    VALUES(
                        @ZoneId,@CustomerNumber,@Barge,
                        @RegisterDateJalali,@PrviousDateJalali,@CurrentDateJalali,@PreviousNumber,@CurrentNumber,
                        @Consumption,@AbBahaAmount,@FazelabAmount,@Baha,
                        @BillId,@PaymentId,@ToDayDateJalali,0)";
        }
        private string GetDbName(int zoneId)
        {
            return zoneId > 140000 ? "Abfar" : zoneId.ToString();
        }
    }
}
