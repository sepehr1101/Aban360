using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Implementations
{
    internal sealed class TankerWaterDistanceTariffDeleteHadler : ITankerWaterDistanceTariffDeleteHadler
    {
        private readonly ITankerWaterDistanceTariffCommandService _commandService;
        public TankerWaterDistanceTariffDeleteHadler(
            ITankerWaterDistanceTariffCommandService commandService)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(SearchById input, IAppUser appUser, CancellationToken cancellationToken)
        {
            DeleteDto deleteDto = new(input.Id, DateTime.Now, appUser.UserId);
            await _commandService.Delete(deleteDto);
        }
    }
}
