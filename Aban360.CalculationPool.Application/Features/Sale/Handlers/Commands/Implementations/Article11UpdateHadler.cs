using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Implementations
{
    internal sealed class Article11UpdateHadler : IArticle11UpdateHadler
    {
        private readonly IArticle11CommandService _commandService;
        private readonly IValidator<Article11UpdateDto> _validator;
        public Article11UpdateHadler(
            IArticle11CommandService commandService,
            IValidator<Article11UpdateDto> validator)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(Article11UpdateDto inputDto, IAppUser appuser, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            Article11InputDto article11Create = GetArticle11(inputDto, appuser.UserId);
            DeleteDto article11Delete = new(inputDto.Id, DateTime.Now, appuser.UserId);
            await _commandService.Update(article11Create, article11Delete);
        }
        private Article11InputDto GetArticle11(Article11UpdateDto input, Guid userId)
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
