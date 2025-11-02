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
            await _sqlConnection.ExecuteScalarAsync(query, input);
        }
        public async Task Update(Article11InputDto create, DeleteDto delete)
        {
            string deleteQuery = DeleteQuery();
            string createQuery = CreateQuery();

            using (var connection = _sqlConnection)
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteScalarAsync(createQuery, create, transaction);
                    await connection.ExecuteScalarAsync(deleteQuery, delete, transaction);

                    transaction.Commit();
                }
                connection.Close();
            }
        }
        public async Task Delete(DeleteDto input)
        {
            string query = DeleteQuery();
            await _sqlConnection.ExecuteScalarAsync(query, input);
        }

        private string CreateQuery()
        {
            return @"INSERT INTO [Aban360].CalculationPool.Article11(
                        DomesticWaterAmount,DomesticSewageAmount,NonDomesticWaterAmount,NonDomesticSewageAmount,
                        BlockCode,ZoneId,FromDateJalali,ToDateJalali,
						RegisterDateTime,RegisterByUserId,RemoveDateTime,RemoveByUserId
                    )
                    VALUES (
                        @DomesticWaterAmount,@DomesticSewageAmount,@NonDomesticWaterAmount,@NonDomesticSewageAmount,
                        @BlockCode,@ZoneId,@FromDateJalali,@ToDateJalali,
                        @RegisterDateTime,@RegisterByUserId,@RemoveDateTime,@RemoveByUserId)";
        }

        private string DeleteQuery()
        {
                return @"UPDATE [Aban360].CalculationPool.Article11
                        SET
                            RemoveDateTime = @RemoveDateTime ,
						    RemoveByUserId= @RemoveByUserId
                        WHERE
                            Id = @Id AND
						    RemoveDateTime IS NULL;";
        }
    }
}
