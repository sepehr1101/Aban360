using System.Data;
using System.Text.RegularExpressions;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Command.Implementations;
using Dapper;
using Microsoft.Data.SqlClient;

internal static class SqlIntegrationChecks
{
    public static async Task Run(Action<bool, string> check)
    {
        const string masterConnection = @"Server=(localdb)\Aban360MinutesReview;Database=master;Integrated Security=true;TrustServerCertificate=true";
        const string atlasConnection = @"Server=(localdb)\Aban360MinutesReview;Database=Atlas;Integrated Security=true;TrustServerCertificate=true";
        var directory = Path.Combine(AppContext.BaseDirectory, "sql-review");
        Directory.CreateDirectory(directory);
        string dataFile = Path.Combine(directory, "Atlas.mdf").Replace("'", "''");
        string logFile = Path.Combine(directory, "Atlas_log.ldf").Replace("'", "''");
        using var setup = new SqlConnection(masterConnection);
        await setup.OpenAsync();
        if (await setup.ExecuteScalarAsync<int?>("SELECT DB_ID('Atlas')") is not null)
            throw new InvalidOperationException("The isolated review instance must have no existing Atlas database.");
        await setup.ExecuteAsync($@"CREATE DATABASE Atlas ON PRIMARY
            (NAME=N'Atlas', FILENAME=N'{dataFile}')
            LOG ON (NAME=N'Atlas_log', FILENAME=N'{logFile}')");
        try
        {
        await setup.ChangeDatabaseAsync("Atlas");
        await setup.ExecuteAsync(@"CREATE TABLE dbo.autoback (id int IDENTITY PRIMARY KEY, jalase_no decimal(10,0));
            CREATE TABLE dbo.repair (id int IDENTITY PRIMARY KEY, jalase_no decimal(10,0));
            INSERT dbo.autoback (jalase_no) VALUES (100);
            INSERT dbo.repair (jalase_no) VALUES (200);
            CREATE TABLE dbo.ReviewReturns (Number int NOT NULL PRIMARY KEY);");
        string scriptPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
            "../../../../../Aban360.OldCalcPool.Persistence/Migrations/Manual/Atlas/002_ReturnBillMinutesCounter.sql"));
        string script = await File.ReadAllTextAsync(scriptPath);
        foreach (string batch in Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
            if (!string.IsNullOrWhiteSpace(batch)) await setup.ExecuteAsync(batch);
        check(await setup.ExecuteScalarAsync<int>("SELECT LastNumber FROM dbo.ReturnBillMinutesCounter WHERE Id=1") == 200,
            "SQL migration seeds above both historical tables");

        int rolledBackNumber;
        using (var connection = new SqlConnection(atlasConnection))
        {
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
            rolledBackNumber = await new ReturnBillMinutesCounterCommandService(connection, transaction).GetNext();
            await connection.ExecuteAsync("INSERT dbo.ReviewReturns VALUES (@number)", new { number = rolledBackNumber }, transaction);
            transaction.Rollback();
        }
        check(await setup.ExecuteScalarAsync<int>("SELECT LastNumber FROM dbo.ReturnBillMinutesCounter WHERE Id=1") == 200
            && await setup.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM dbo.ReviewReturns") == 0,
            "SQL rollback restores both the counter and the return record");

        using (var connection = new SqlConnection(atlasConnection))
        {
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
            int number = await new ReturnBillMinutesCounterCommandService(connection, transaction).GetNext();
            check(number == rolledBackNumber, "Rolled-back allocation is available for the next successful return");
            await connection.ExecuteAsync("INSERT dbo.ReviewReturns VALUES (@number)", new { number }, transaction);
            transaction.Commit();
        }

        var numbers = await Task.WhenAll(Enumerable.Range(0, 20).Select(async _ =>
        {
            using var connection = new SqlConnection(atlasConnection);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
            int number = await new ReturnBillMinutesCounterCommandService(connection, transaction).GetNext();
            await connection.ExecuteAsync("INSERT dbo.ReviewReturns VALUES (@number)", new { number }, transaction);
            transaction.Commit();
            return number;
        }));
        check(numbers.Distinct().Count() == 20 && numbers.Min() == 202 && numbers.Max() == 221,
            "Twenty concurrent SQL transactions commit unique consecutive numbers");
        check(await setup.ExecuteScalarAsync<int>("SELECT LastNumber FROM dbo.ReturnBillMinutesCounter WHERE Id=1") == 221
            && await setup.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM dbo.ReviewReturns") == 21,
            "Persisted counter agrees with the committed return records");

        try
        {
            using var connection = new SqlConnection(atlasConnection);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
            int number = await new ReturnBillMinutesCounterCommandService(connection, transaction).GetNext();
            await connection.ExecuteAsync(@"INSERT dbo.ReviewReturns VALUES (@number);
                INSERT dbo.ReviewReturns VALUES (@number);", new { number }, transaction);
            throw new Exception("Duplicate-key insertion must fail");
        }
        catch (SqlException exception) when (exception.Number is 2627 or 2601)
        {
            check(await setup.ExecuteScalarAsync<int>("SELECT LastNumber FROM dbo.ReturnBillMinutesCounter WHERE Id=1") == 221
                && await setup.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM dbo.ReviewReturns") == 21,
                "A SQL write failure disposes the transaction and rolls back allocation and inserts");
        }

        foreach (string batch in Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
            if (!string.IsNullOrWhiteSpace(batch)) await setup.ExecuteAsync(batch);
        check(await setup.ExecuteScalarAsync<int>("SELECT LastNumber FROM dbo.ReturnBillMinutesCounter WHERE Id=1") == 221,
            "Rerunning migration preserves an existing counter");
        }
        finally
        {
            await setup.ChangeDatabaseAsync("master");
            SqlConnection.ClearAllPools();
            await setup.ExecuteAsync(@"ALTER DATABASE Atlas SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                DROP DATABASE Atlas;");
        }
    }
}
