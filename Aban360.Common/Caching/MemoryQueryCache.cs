using Microsoft.Extensions.Caching.Memory;
using System.Buffers;
using System.Collections.Concurrent;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.Json;

namespace Aban360.Common.Caching;

public sealed class MemoryQueryCache : IQueryCache, IDisposable
{
    private readonly MemoryCache _cache = new(new MemoryCacheOptions { SizeLimit = 128 });
    private readonly ConcurrentDictionary<CacheKey, TaskCompletionSource<object>> _inFlight = new();

    public async Task<TResult> GetOrCreateAsync<TInput, TResult>(
        string operation,
        TInput input,
        TimeSpan expiration,
        Func<TInput, CancellationToken, Task<TResult>> create,
        CancellationToken cancellationToken)
        where TInput : notnull
        where TResult : notnull
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentException.ThrowIfNullOrWhiteSpace(operation);
        ArgumentNullException.ThrowIfNull(input);
        Type inputType = input.GetType();
        var cacheKey = new CacheKey(operation, inputType, typeof(TResult),
            CultureInfo.CurrentCulture.Name, CultureInfo.CurrentUICulture.Name, HashInput(input, inputType));

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_cache.TryGetValue<TResult>(cacheKey, out var cached))
                return cached!;

            var candidate = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            var pending = _inFlight.GetOrAdd(cacheKey, candidate);
            if (ReferenceEquals(pending, candidate))
            {
                try
                {
                    // Another request may have completed between the lookup and registration.
                    if (!_cache.TryGetValue<TResult>(cacheKey, out var result))
                    {
                        result = await create(input, cancellationToken);
                        cancellationToken.ThrowIfCancellationRequested();
                        _cache.Set(cacheKey, result, new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = expiration,
                            Size = 1
                        });
                    }
                    candidate.SetResult(result!);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    candidate.SetCanceled(cancellationToken);
                }
                catch (Exception error)
                {
                    candidate.SetException(error);
                }
                finally
                {
                    _inFlight.TryRemove(cacheKey, out _);
                }
                return (TResult)await candidate.Task;
            }

            try
            {
                return (TResult)await pending.Task.WaitAsync(cancellationToken);
            }
            catch (OperationCanceledException) when (pending.Task.IsCanceled && !cancellationToken.IsCancellationRequested)
            {
                // The producer was canceled. This request can start a new computation.
            }
        }
    }

    private static string HashInput(object input, Type inputType)
    {
        var buffer = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(buffer);
        WriteCanonicalJson(writer, JsonSerializer.SerializeToElement(input, inputType));
        writer.Flush();
        return Convert.ToHexString(SHA256.HashData(buffer.WrittenSpan));
    }

    private static void WriteCanonicalJson(Utf8JsonWriter writer, JsonElement value)
    {
        switch (value.ValueKind)
        {
            case JsonValueKind.Object:
                writer.WriteStartObject();
                foreach (var property in value.EnumerateObject().OrderBy(property => property.Name, StringComparer.Ordinal))
                {
                    writer.WritePropertyName(property.Name);
                    WriteCanonicalJson(writer, property.Value);
                }
                writer.WriteEndObject();
                break;
            case JsonValueKind.Array:
                writer.WriteStartArray();
                foreach (var item in value.EnumerateArray())
                    WriteCanonicalJson(writer, item);
                writer.WriteEndArray();
                break;
            default:
                value.WriteTo(writer);
                break;
        }
    }

    public void Dispose() => _cache.Dispose();

    private readonly record struct CacheKey(
        string Operation, Type InputType, Type ResultType, string Culture, string UICulture, string InputHash);
}
