using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Application.Features.TotalApi.Handler
{
    public interface ITotalApiCommandService
    {
        Task Handle(TotalApiCreateDto createDto, CancellationToken cancellationToken);
    }
}
