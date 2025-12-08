using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Command.Contracts;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class ReturnedBillHandler : IReturnedBillHandler
    {
        private readonly IRepairCommandService _repairCommandService;
        public ReturnedBillHandler(IRepairCommandService repairCommandService)
        {
            _repairCommandService = repairCommandService;
            _repairCommandService.NotNull(nameof(repairCommandService));
        }

        public async Task Handle(ReturnedBillInputDto inputDto, CancellationToken cancellationToken)
        {
            //validation
            if (inputDto.ReturnCauseId is null)
            {
                // دستی
                //کل قبض های از/تا تاریخ برگشت میخوره
            }
            else//راه برگشتی داره
            {
                if (inputDto.ReturnCauseId.Value == 1)//code=1 & id=25
                {
                    //محاسبه به روش لوله ترکیدگی
                }
                else
                {
                    //راههای دیگر؟؟
                }
            }
        }
        private async Task ReturnedManual()
        {
            //
        }
    }
}
