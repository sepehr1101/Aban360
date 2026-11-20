using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Exceptions;

namespace Aban360.CalculationPool.Application.Features.Sale.Services
{
    public interface IArticle11Service
    {
        Task<IEnumerable<Article11OutputDto>> Article11GetAllAsync(CancellationToken cancellationToken);
        Task<Article11OutputDto> Article11GetAsync(SearchById input, CancellationToken cancellationToken);
    }

    internal sealed class Article11Service : IArticle11Service
    {
        private readonly IArticle11QueryService _queryService;
        private readonly IValidator<SearchById> _validator;
        public Article11Service(IArticle11QueryService queryService, IValidator<SearchById> validator)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<IEnumerable<Article11OutputDto>> Article11GetAllAsync(CancellationToken cancellationToken)
        {
            IEnumerable<Article11OutputDto> result = await _queryService.Get(DateTime.Now.ToShortPersianDateString());
            return result;
        }

        public async Task<Article11OutputDto> Article11GetAsync(SearchById input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            Article11OutputDto result = await _queryService.Get(input.Id, DateTime.Now.ToShortPersianDateString());
            return result;
        }
    }
}
