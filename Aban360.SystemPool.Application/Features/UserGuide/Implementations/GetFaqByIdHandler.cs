using Aban360.SystemPool.Application.Features.UserGuide.Contracts;
using Aban360.SystemPool.Domain.Features.UserGuide.Entities;
using Aban360.SystemPool.Persistence.Features.UserGuide.Contracts;

namespace Aban360.SystemPool.Application.Features.UserGuide.Implementations
{
    internal sealed class GetFaqByIdHandler : IGetFaqByIdHandler
    {
        private readonly IFaqService _faqService;

        public GetFaqByIdHandler(IFaqService faqService)
        {
            _faqService = faqService;
        }

        public async Task<Faq?> Handle(int id, CancellationToken cancellationToken)
        {
            return await _faqService.Get(id);
        }
    }

}
