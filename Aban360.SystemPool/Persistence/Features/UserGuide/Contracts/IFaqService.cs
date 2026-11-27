using Aban360.SystemPool.Domain.Features.UserGuide.Dtos;
using Aban360.SystemPool.Domain.Features.UserGuide.Entities;

namespace Aban360.SystemPool.Persistence.Features.UserGuide.Contracts
{
    public interface IFaqService
    {
        Task<int> Create(Faq faq);
        Task<int> Delete(int id, string deletedBy);
        Task<IEnumerable<FaqGetAllDto>> Get();
        Task<Faq?> Get(int id);
        Task<int> Update(FaqDto faq);
    }
}