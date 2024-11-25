using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aban360.UserPool.Persistence.Interceptors
{
    public class RowLevelAuthenticityICommandInterceptor: DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(
          DbCommand command,
          CommandEventData eventData,
          InterceptionResult<DbDataReader> result)
        {
            return base.ReaderExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
           DbCommand command,
           CommandEventData eventData,
           InterceptionResult<DbDataReader> result,
           CancellationToken cancellationToken = new())
        {
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);    
        }

        public override InterceptionResult<int> NonQueryExecuting(
           DbCommand command,
           CommandEventData eventData,
           InterceptionResult<int> result)
        {
            return base.NonQueryExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(
           DbCommand command,
           CommandEventData eventData,
           InterceptionResult<int> result,
           CancellationToken cancellationToken = new())
        {
            return base.NonQueryExecutingAsync(command,eventData,result, cancellationToken)     
        }

        public override InterceptionResult<object> ScalarExecuting(
           DbCommand command,
           CommandEventData eventData,
           InterceptionResult<object> result)
        {
            return base.ScalarExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<object> result,
            CancellationToken cancellationToken = new())
        {
            base.ScalarExecuting(command, eventData, result);
        }
    }
}
