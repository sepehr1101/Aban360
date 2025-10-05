using Aban360.Common.ApplicationUser;
using Aban360.SystemPool.Application.Features.UserGuide.Contracts;
using Aban360.SystemPool.Domain.Features.UserGuide.Dtos;
using Aban360.SystemPool.Domain.Features.UserGuide.Entities;
using Aban360.SystemPool.Persistence.Features.UserGuide.Contracts;

namespace Aban360.SystemPool.Application.Features.UserGuide.Implementations
{
    internal sealed class CreateFaqHandler : ICreateFaqHandler
    {
        private readonly IFaqService _faqService;

        public CreateFaqHandler(IFaqService faqService)
        {
            _faqService = faqService;
        }

        public async Task<int> Handle(FaqDto faqDto, IAppUser currentUser, CancellationToken cancellationToken)
        {
            Faq faq= Map(faqDto, currentUser);
            return await _faqService.Create(faq);
        }
        private Faq Map(FaqDto faq, IAppUser currentUser)
        {
            return new Faq()
            {
                Content = faq.Content,
                CreateDateTime = DateTime.Now,
                CreatedBy = currentUser.FullName,
                Header = faq.Header,
                Icon = faq.Icon
            };
        }
    }

}
