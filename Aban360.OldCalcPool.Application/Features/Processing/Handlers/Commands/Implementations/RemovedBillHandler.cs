using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class RemovedBillHandler : IRemovedBillHandler
    {
        private readonly IBedBesCommandService _bedBesCommandService;
        private readonly IHBedBesCommanddService _hbedBesCommanddService;
        private readonly IKasrHaService _kasrHaService;
        public RemovedBillHandler(
            IBedBesCommandService bedBesCommandService,
            IHBedBesCommanddService hbedBesCommanddService,
            IKasrHaService kasrHaService)
        {
            _bedBesCommandService = bedBesCommandService;
            _bedBesCommandService.NotNull(nameof(bedBesCommandService));

            _hbedBesCommanddService = hbedBesCommanddService;
            _hbedBesCommanddService.NotNull(nameof(hbedBesCommanddService));

            _kasrHaService = kasrHaService;
            _kasrHaService.NotNull(nameof(kasrHaService));
        }

        public async Task Handle(RemovedBillInputDto inputDto, CancellationToken cancellationToken)
        {
            inputDto.ToDayDateJalali = DateTime.Now.ToShortPersianDateString();

            await _bedBesCommandService.Delete(inputDto.Id);
            await _kasrHaService.Delete(inputDto);
            await _hbedBesCommanddService.Insert(inputDto);
        }
    }
}
