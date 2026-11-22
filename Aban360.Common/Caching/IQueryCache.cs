namespace Aban360.Common.Caching;

public interface IQueryCache
{
    // The JSON-serializable input must include every value that affects the result, including access scope.
    // Results are shared. Copy mutable results before exposing them to a caller.
    Task<TResult> GetOrCreateAsync<TInput, TResult>(
        string operation,
        TInput input,
        TimeSpan expiration,
        Func<TInput, CancellationToken, Task<TResult>> create,
        CancellationToken cancellationToken)
        where TInput : notnull
        where TResult : notnull;
}
