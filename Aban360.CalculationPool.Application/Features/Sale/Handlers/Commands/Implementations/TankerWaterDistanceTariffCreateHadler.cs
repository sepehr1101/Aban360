using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Implementations
{
    internal sealed class TankerWaterDistanceTariffCreateHadler : ITankerWaterDistanceTariffCreateHadler
    {
        private readonly ITankerWaterDistanceTariffCommandService _commandService;
        private readonly IValidator<TankerWaterDistanceTariffInputDto> _validator;
        public TankerWaterDistanceTariffCreateHadler(
            ITankerWaterDistanceTariffCommandService commandService,
            IValidator<TankerWaterDistanceTariffInputDto> validator)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(TankerWaterDistanceTariffInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            TankerWaterDistanceTariffCreateDto tankerCreate = GetTankerWaterDistanceTariff(inputDto, appUser.UserId);
            await _commandService.Create(tankerCreate);
        }
        private TankerWaterDistanceTariffCreateDto GetTankerWaterDistanceTariff(TankerWaterDistanceTariffInputDto input, Guid userId)
        {
            return new TankerWaterDistanceTariffCreateDto()
            {
                FromDistance = input.FromDistance,
                ToDistance = input.ToDistance,
                Amount = input.Amount,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RegisterDateTime = DateTime.Now,
                RegisterByUserId = userId
            };
        }
    }
}
