using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    internal sealed class Article11GetAllHandler : IArticle11GetAllHandler
    {
        private readonly IArticle11QueryService _queryService;
        public Article11GetAllHandler(IArticle11QueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<IEnumerable<Article11OutputDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<Article11OutputDto> result = await _queryService.Get(DateTime.Now.ToShortPersianDateString());
            return result;
        }
    }
}
