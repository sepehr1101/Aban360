using Aban360.Common.ApplicationUser;
using Aban360.SystemPool.Domain.Features.UserGuide.Dtos;
using Aban360.SystemPool.Domain.Features.UserGuide.Entities;
using Aban360.SystemPool.Persistence.Features.UserGuide.Contracts;

namespace Aban360.SystemPool.Application.Features.UserGuide.Services;

public interface IFaqApplicationService
{
    Task<int> CreateAsync(FaqDto faqDto, IAppUser currentUser, CancellationToken cancellationToken);
    Task<int> UpdateAsync(FaqDto faq, CancellationToken cancellationToken);
    Task<int> DeleteAsync(int id, IAppUser currentUser, CancellationToken cancellationToken);
    Task<Faq?> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<FaqGetAllDto>> GetAllAsync(CancellationToken cancellationToken);
}

internal sealed class FaqApplicationService : IFaqApplicationService
{
    private readonly IFaqService _faqService;
    public FaqApplicationService(
        IFaqService faqService)
    {
        _faqService = faqService;
    }

    public async Task<int> CreateAsync(FaqDto faqDto, IAppUser currentUser, CancellationToken cancellationToken)
    {
        Faq faq = CreateMap(faqDto, currentUser);
        return await _faqService.Create(faq);
    }

    private Faq CreateMap(FaqDto faq, IAppUser currentUser)
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

    public async Task<int> UpdateAsync(FaqDto faq, CancellationToken cancellationToken)
    {
        return await _faqService.Update(faq);
    }

    public async Task<int> DeleteAsync(int id, IAppUser currentUser, CancellationToken cancellationToken)
    {
        return await _faqService.Delete(id, currentUser.FullName);
    }

    public async Task<Faq?> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        return await _faqService.Get(id);
    }

    public async Task<IEnumerable<FaqGetAllDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _faqService.Get();
    }
}
