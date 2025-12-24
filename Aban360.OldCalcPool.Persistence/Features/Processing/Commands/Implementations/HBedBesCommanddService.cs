using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    internal sealed class HBedBesCommanddService : AbstractBaseConnection, IHBedBesCommanddService
    {
        public HBedBesCommanddService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Insert(RemoveBillDataInputDto input)
        {
            string command = GetInsertCommand();
            await _sqlReportConnection.ExecuteAsync(command, input);
        }

        private string GetInsertCommand()
        {
            return @"INSERT INTO [Atlas].dbo.Hbedbes(
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
    }
}
