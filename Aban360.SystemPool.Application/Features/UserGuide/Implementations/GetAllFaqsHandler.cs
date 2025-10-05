using Aban360.SystemPool.Application.Features.UserGuide.Contracts;
using Aban360.SystemPool.Domain.Features.UserGuide.Dtos;
using Aban360.SystemPool.Persistence.Features.UserGuide.Contracts;

namespace Aban360.SystemPool.Application.Features.UserGuide.Implementations
{
    internal sealed class GetAllFaqsHandler : IGetAllFaqsHandler
    {
        private readonly IFaqService _faqService;

        public GetAllFaqsHandler(IFaqService faqService)
        {
            _faqService = faqService;
        }

        public async Task<IEnumerable<FaqGetAllDto>> Handle(CancellationToken cancellationToken)
        {
            return await _faqService.Get();
        }
    }

}
