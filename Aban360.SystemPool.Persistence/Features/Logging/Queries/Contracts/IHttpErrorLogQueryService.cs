using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;

namespace Aban360.SystemPool.Persistence.Features.Logging.Queries.Contracts;

public interface IHttpErrorLogQueryService
{
    Task<IEnumerable<HttpErrorLogOutput>> Get(DateTime from, DateTime toExclusive,
        int statusCodeFrom, int statusCodeToExclusive, int page, int pageSize, CancellationToken cancellationToken);
}
