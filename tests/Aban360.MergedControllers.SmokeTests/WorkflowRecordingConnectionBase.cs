using System.Data;
using Microsoft.Extensions.Configuration;

internal abstract class WorkflowRecordingConnectionBase : Aban360.Common.Db.Dapper.AbstractBaseConnection
{
    internal static readonly AsyncLocal<AuxiliarySqlState?> Current=new();
    readonly AuxiliarySqlState state;
    protected WorkflowRecordingConnectionBase(IConfiguration configuration):base(configuration)=>state=Current.Value??throw new InvalidOperationException("Missing isolated workflow SQL fixture");
    protected new IDbConnection _sqlConnection {get{state.Main.FactoryCalls++;return state.Main.Connection;}}
    protected new IDbConnection _sqlReportConnection {get{state.Report.FactoryCalls++;return state.Report.Connection;}}
}
