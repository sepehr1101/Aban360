using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    internal sealed class Article11GetAllHadler : IArticle11GetAllHadler
    {
        private readonly IArticle11QueryService _queryService;
        public Article11GetAllHadler(IArticle11QueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<IEnumerable<Article11OutputDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<Article11OutputDto> result = await _queryService.Get();
            return result;
        }
    }
}
