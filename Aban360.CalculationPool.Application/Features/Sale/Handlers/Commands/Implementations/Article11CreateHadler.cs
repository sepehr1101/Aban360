using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Implementations
{
    internal sealed class Article11CreateHadler : IArticle11CreateHadler
    {
        private readonly IArticle11CommandService _commandService;
        private readonly IValidator<Article11InputDto> _validator;
        public Article11CreateHadler(
            IArticle11CommandService commandService,
            IValidator<Article11InputDto> validator)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(Article11InputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            inputDto.RegisterDateJalali = DateTime.Now.ToShortPersianDateString();
           // inputDto.IsDomestic = inputDto.IsDomestic ? 1 : 0;
            await _commandService.Create(inputDto);
        }
    }
}
