using Aban360.SystemPool.Domain.Features.UserGuide.Entities;

namespace Aban360.SystemPool.Application.Features.UserGuide.Contracts
{
    public interface IGetFaqByIdHandler
    {
        Task<Faq?> Handle(int id, CancellationToken cancellationToken);
    }
}