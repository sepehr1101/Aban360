using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts
{
    public interface IArticle11QueryService
    {
        Task<Article11OutputDto> Get(Article11GetDto input);
        Task<Article11OutputDto> Get(short id, string currentDateJalali);
        Task<IEnumerable<Article11OutputDto>> Get(string currentDateJalali);
    }
}
