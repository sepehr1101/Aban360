using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    public static class UserZoneIdsQueryService
    {
        static string _connectionString = "Server=.;Database=Aban360;Trusted_Connection=True; Application Name=Aban360;MultipleActiveResultSets=True; TrustServerCertificate=true;";
        public static async Task<ICollection<UserZoneIdsOutputDto>> GetInfo(Guid id, short claimTypeId)
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
                        var value = reader.GetString(0);
                        zoneIds.Add(int.Parse(value));
                    }
                }
            }
            //
            ICollection<UserZoneIdsOutputDto> result = new List<UserZoneIdsOutputDto>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var paramNames = zoneIds.Select((id, i) => $"@zoneId{i}").ToList();
                string getZoneQueryString = GetUserZonesQuery(paramNames);

                using (var command = new SqlCommand(getZoneQueryString, connection))
                {
                    int i = 0;
                    foreach (var item in zoneIds)
                    {
                        command.Parameters.AddWithValue($"@zoneId{i}", item);
                        i++;
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new UserZoneIdsOutputDto()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title"))
                            });
                        }
                    }
                }

            }
            return result;

        }

        private static string GetUserZoneIdsQuery()
        {
            return @"Select u.ClaimValue
                     From [Aban360].UserPool.UserClaim u
                     Where 
                        u.UserId=@userId AND 
                        u.ClaimTypeId=@claimTypeId";
        }

        private static string GetUserZonesQuery(IEnumerable<string> parmeters)
        {
            string param=string.Join(", ", parmeters);
            return @$"select z.Id,z.Title
                     from LocationPool.Zone z
                     where z.Id IN ({param})";
        }
    }
}


