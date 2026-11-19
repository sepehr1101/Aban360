using System.Data;
using Microsoft.Data.SqlClient;

// Test-only boundary: execute the original ToDataTable and column mappings, then
// record destination, options, schema and every row on the existing SQL provider.
internal sealed class WorkflowRecordingBulkCopy(IDbConnection connection,SqlBulkCopyOptions options,IDbTransaction transaction):IDisposable
{
    public string DestinationTableName{get;set;}="";
    public int BatchSize{get;set;}
    public int BulkCopyTimeout{get;set;}
    public WorkflowColumnMappings ColumnMappings{get;}=new();
    public async Task WriteToServerAsync(DataTable table)
    {
        var state=WorkflowRecordingConnectionBase.Current.Value!.Report;
        using var command=(RecordingCommand)connection.CreateCommand();command.Transaction=(System.Data.Common.DbTransaction)transaction;
        command.CommandText="BULK INSERT "+DestinationTableName;
        void Add(string name,object value){var p=command.CreateParameter();p.ParameterName=name;p.Value=value;command.Parameters.Add(p);}
        Add("options",options.ToString());Add("batch-size",BatchSize);Add("timeout",BulkCopyTimeout);Add("row-count",table.Rows.Count);
        Add("schema",string.Join("|",table.Columns.Cast<DataColumn>().Select(c=>c.ColumnName+":"+c.DataType.FullName)));
        Add("mappings",string.Join("|",ColumnMappings.Items.Select(p=>p.Source+"=>"+p.Target)));
        for(int i=0;i<table.Rows.Count;i++)foreach(DataColumn column in table.Columns)Add("row"+i+"."+column.ColumnName,table.Rows[i][column]);
        await state.Execute(command);
    }
    public void Dispose()=>WorkflowRecordingConnectionBase.Current.Value!.Report.Record("DisposeBulkCopy:"+DestinationTableName);
}
internal sealed class WorkflowColumnMappings
{
    public List<(string Source,string Target)> Items{get;}=[];
    public void Add(string source,string target)=>Items.Add((source,target));
}
