using Aban360.SystemPool.Domain.Features.UserGuide.Dtos;

namespace Aban360.SystemPool.Application.Features.UserGuide.Contracts
{
    public interface IUpdateFaqHandler
    {
        Task<int> Handle(FaqDto faq, CancellationToken cancellationToken);
    }
}