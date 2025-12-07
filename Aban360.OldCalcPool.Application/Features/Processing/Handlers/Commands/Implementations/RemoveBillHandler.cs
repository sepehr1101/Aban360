using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class RemoveBillHandler : IRemoveBillHandler
    {
        private readonly IBedBesCommandService _bedBesCommandService;
        private readonly IHBedBesCommanddService _hbedBesCommanddService;
        private readonly IKasrHaService _kasrHaService;
        private readonly IBillQueryService _billQueryService;
        public RemoveBillHandler(
            IBedBesCommandService bedBesCommandService,
            IHBedBesCommanddService hbedBesCommanddService,
            IKasrHaService kasrHaService,
            IBillQueryService billQueryService)
        {
            _bedBesCommandService = bedBesCommandService;
            _bedBesCommandService.NotNull(nameof(bedBesCommandService));

            _hbedBesCommanddService = hbedBesCommanddService;
            _hbedBesCommanddService.NotNull(nameof(hbedBesCommanddService));

            _kasrHaService = kasrHaService;
            _kasrHaService.NotNull(nameof(kasrHaService));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));
        }

        public async Task Handle(int id, CancellationToken cancellationToken)
        {
            RemoveBillInputDto removeBill=await GetRemoveBillInputDto(id);
            removeBill.ToDayDateJalali = DateTime.Now.ToShortPersianDateString();

            await _bedBesCommandService.Delete(removeBill.Id);
            await _kasrHaService.Delete(removeBill);
            await _hbedBesCommanddService.Insert(removeBill);
        }
        public async Task<RemoveBillInputDto> GetRemoveBillInputDto(int id)
        {
            RemoveBillInputDto result = await _billQueryService.GetToRemove(id);
            return result;
        }
    }
}