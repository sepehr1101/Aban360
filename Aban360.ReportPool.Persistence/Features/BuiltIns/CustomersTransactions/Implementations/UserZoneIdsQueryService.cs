using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    public static class UserZoneIdsQueryService
    {
        static string _connectionString = "Server=.;Database=Aban360;Trusted_Connection=True; Application Name=Aban360;MultipleActiveResultSets=True; TrustServerCertificate=true;";
        public static async Task<List<int>> GetInfo(Guid id,short claimTypeId)
        {
            List<int> zoneIds = new List<int>();

            await using (SqlConnection connection = new SqlConnection(_connectionString))
            await using (SqlCommand command = new SqlCommand(GetUserZoneIdsQuery(), connection))
            {
                command.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = id;
                command.Parameters.Add("@claimTypeId", SqlDbType.SmallInt).Value = claimTypeId;
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                { 
                    while (await reader.ReadAsync())
                    {
                        zoneIds.Add(reader.GetInt32(0));    
                    }
                }
            }

            return zoneIds;
        }

        private static string GetUserZoneIdsQuery()
        {
            return @"Select u.ClaimValue
                     From [Aban360].UserPool.UserClaim u
                     Where 
                        u.UserId=@userId AND 
                        u.ClaimTypeId=@claimTypeId";
        }
    }
}


