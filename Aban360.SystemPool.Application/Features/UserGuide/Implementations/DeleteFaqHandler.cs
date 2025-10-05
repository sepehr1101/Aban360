using Aban360.Common.ApplicationUser;
using Aban360.SystemPool.Application.Features.UserGuide.Contracts;
using Aban360.SystemPool.Persistence.Features.UserGuide.Contracts;

namespace Aban360.SystemPool.Application.Features.UserGuide.Implementations
{
    internal sealed class DeleteFaqHandler : IDeleteFaqHandler
    {
        private readonly IFaqService _faqService;

        public DeleteFaqHandler(IFaqService faqService)
        {
            _faqService = faqService;
        }

        public async Task<int> Handle(int id, IAppUser currentUser, CancellationToken cancellationToken)
        {
            return await _faqService.Delete(id,currentUser.FullName);
        }
    }

}
