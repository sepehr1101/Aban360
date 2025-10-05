using Aban360.SystemPool.Application.Features.UserGuide.Contracts;
using Aban360.SystemPool.Domain.Features.UserGuide.Dtos;
using Aban360.SystemPool.Persistence.Features.UserGuide.Contracts;

namespace Aban360.SystemPool.Application.Features.UserGuide.Implementations
{
    internal sealed class UpdateFaqHandler : IUpdateFaqHandler
    {
        private readonly IFaqService _faqService;

        public UpdateFaqHandler(IFaqService faqService)
        {
            _faqService = faqService;
        }

        public async Task<int> Handle(FaqDto faq, CancellationToken cancellationToken)
        {
            return await _faqService.Update(faq);
        }
    }

}
