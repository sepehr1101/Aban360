using System.Data;
using System.Data.Common;

// Native SqlCommand boundary used only by exact copies of bulk command services.
// The source SQL is executed by the same recording provider as real Dapper calls.
internal sealed class Operations19RecordingSqlCommand(string sql,IDbConnection connection,IDbTransaction transaction)
{
    public async Task<int> ExecuteNonQueryAsync()
    {
        using var command=(DbCommand)connection.CreateCommand();command.CommandText=sql;command.Transaction=(DbTransaction)transaction;
        return await command.ExecuteNonQueryAsync();
    }
}
