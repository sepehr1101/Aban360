using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Commands.Implementations
{
    internal sealed class Article11CommandService : AbstractBaseConnection, IArticle11CommandService
    {
        public Article11CommandService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Create(Article11InputDto input)
        {
            string query = CreateQuery();
            await _sqlReportConnection.ExecuteScalarAsync(query, input);
        }
        public async Task Update(Article11UpdateDto input)
        {
            string query = UpdateQuery();
            await _sqlReportConnection.ExecuteScalarAsync(query, input);
        }
        public async Task Delete(DeleteDto input)
        {
            string query = DeleteQuery();
            await _sqlReportConnection.ExecuteScalarAsync(query, input);
        }

        private string CreateQuery()
        {
            return @"INSERT INTO [Aban360].CalculationPool.Article11(
                        WaterMeterAmount,WaterAmount,
                        SewageMeterAmount,SewageAmount,IsDomestic,
                        BlockCode,ZoneId,RegisterDateJalali,
                        FromDateJalali,ToDateJalali,RemovedDateJalali
                    )
                    VALUES (
                        @WaterMeterAmount,@WaterAmount,
                        @SewageMeterAmount,@SewageAmount,@IsDomestic,
                        @BlockCode,@ZoneId,@RegisterDateJalali,
                        @FromDateJalali,@ToDateJalali,@RemovedDateJalali)";
        }

        private string UpdateQuery()
        {
            return @"UPDATE [Aban360].CalculationPool.Article11
                    SET
                        WaterMeterAmount   = @WaterMeterAmount,
                        WaterAmount        = @WaterAmount,
                        SewageMeterAmount  = @SewageMeterAmount,
                        SewageAmount       = @SewageAmount,
                        IsDomestic         = @IsDomestic,
                        BlockCode          = @BlockCode,
                        ZoneId             = @ZoneId,
                        RegisterDateJalali = @RegisterDateJalali,
                        FromDateJalali     = @FromDateJalali,
                        ToDateJalali       = @ToDateJalali
                    WHERE
                        Id = @Id;";
        }

        private string DeleteQuery()
        {
            return @"UPDATE [Aban360].CalculationPool.Article11
                    SET
                        RemovedDateJalali  = @RemovedDateJalali
                    WHERE
                        Id = @Id;";
        }
    }
}
