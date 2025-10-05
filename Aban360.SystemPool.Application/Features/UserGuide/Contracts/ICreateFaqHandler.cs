using Aban360.Common.ApplicationUser;
using Aban360.SystemPool.Domain.Features.UserGuide.Dtos;

namespace Aban360.SystemPool.Application.Features.UserGuide.Contracts
{
    public interface ICreateFaqHandler
    {
        Task<int> Handle(FaqDto faqDto, IAppUser currentUser, CancellationToken cancellationToken);
    }
}