using Aban360.SystemPool.Domain.Features.UserGuide.Dtos;

namespace Aban360.SystemPool.Application.Features.UserGuide.Contracts
{
    public interface IGetAllFaqsHandler
    {
        Task<IEnumerable<FaqGetAllDto>> Handle(CancellationToken cancellationToken);
    }
}