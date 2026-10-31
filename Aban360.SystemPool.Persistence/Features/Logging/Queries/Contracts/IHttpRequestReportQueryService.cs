using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;

namespace Aban360.SystemPool.Persistence.Features.Logging.Queries.Contracts;

public interface IHttpRequestReportQueryService
{
    Task<IEnumerable<HttpRequestReportOutput>> Get(DateTime from, DateTime to,
        HttpRequestReportInput input, HttpRequestReportKind kind, CancellationToken cancellationToken);
}
