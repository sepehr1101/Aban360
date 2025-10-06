using Aban360.Common.ApplicationUser;

namespace Aban360.SystemPool.Application.Features.UserGuide.Contracts
{
    public interface IDeleteFaqHandler
    {
        Task<int> Handle(int id, IAppUser currentUser, CancellationToken cancellationToken);
    }
}