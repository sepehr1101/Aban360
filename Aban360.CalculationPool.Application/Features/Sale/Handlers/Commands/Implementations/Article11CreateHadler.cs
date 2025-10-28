using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Implementations
{
    internal sealed class Article11CreateHadler : IArticle11CreateHadler
    {
        private readonly IArticle11CommandService _commandService;
        private readonly IValidator<Article11CreateDto> _validator;
        public Article11CreateHadler(
            IArticle11CommandService commandService,
            IValidator<Article11CreateDto> validator)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(Article11CreateDto inputDto,IAppUser appUser, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            Article11InputDto article11Input = GetArticle11(inputDto,appUser.UserId);
            await _commandService.Create(article11Input);
        }
        private Article11InputDto GetArticle11(Article11CreateDto input,Guid userId)
        {
            return new Article11InputDto()
            {
                WaterMeterAmount = input.WaterMeterAmount,
                WaterAmount = input.WaterAmount,
                SewageMeterAmount = input.SewageMeterAmount,
                SewageAmount = input.SewageAmount,
                IsDomestic = input.IsDomestic,
                BlockCode = input.BlockCode,
                ZoneId = input.ZoneId,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RegisterDateTime = DateTime.Now,
                RegisterByUserId = userId
            };
        }
    }
}
