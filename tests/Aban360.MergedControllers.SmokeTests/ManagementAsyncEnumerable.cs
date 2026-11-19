using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

// In-memory EF asynchronous query fixture for the unchanged Gridify pipeline.
internal sealed class ManagementAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public ManagementAsyncEnumerable(IEnumerable<T> enumerable):base(enumerable){}
    public ManagementAsyncEnumerable(Expression expression):base(expression){}
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken=default)=>new Enumerator(this.AsEnumerable().GetEnumerator(),cancellationToken);
    IQueryProvider IQueryable.Provider=>new Provider(this);
    sealed class Enumerator(IEnumerator<T> inner,CancellationToken token):IAsyncEnumerator<T>
    {
        public T Current=>inner.Current;
        public ValueTask DisposeAsync(){inner.Dispose();return ValueTask.CompletedTask;}
        public ValueTask<bool> MoveNextAsync(){token.ThrowIfCancellationRequested();return ValueTask.FromResult(inner.MoveNext());}
    }
    sealed class Provider(IQueryProvider inner):IAsyncQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)=>new ManagementAsyncEnumerable<T>(expression);
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)=>new ManagementAsyncEnumerable<TElement>(expression);
        public object? Execute(Expression expression)=>inner.Execute(expression);
        public TResult Execute<TResult>(Expression expression)=>inner.Execute<TResult>(expression);
        public TResult ExecuteAsync<TResult>(Expression expression,CancellationToken cancellationToken=default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result=inner.Execute(expression);
            return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!.MakeGenericMethod(typeof(TResult).GetGenericArguments().Single()).Invoke(null,[result])!;
        }
    }
}
