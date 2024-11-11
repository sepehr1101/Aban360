using System.Transactions;

namespace Aban360.Common.Extensions
{
    public static class TransactionBuilder
    {
        public static TransactionScope Create(int minuts, int seconds = 0, IsolationLevel isolationLevel = IsolationLevel.Snapshot, bool supportAsync = true)
        {
            var options = GetTransactionOption(minuts, seconds, isolationLevel);
            var transactionScope = new TransactionScope(
                TransactionScopeOption.Required,
                options,
                supportAsync ? TransactionScopeAsyncFlowOption.Enabled : TransactionScopeAsyncFlowOption.Suppress);
            return transactionScope;
        }
        private static TransactionOptions GetTransactionOption(int miniuts, int seconds, IsolationLevel isolationLevel)
        {
            var options = new TransactionOptions
            {
                IsolationLevel = isolationLevel,
                Timeout = new TimeSpan(0, miniuts, seconds)
            };
            return options;
        }
    }
}
