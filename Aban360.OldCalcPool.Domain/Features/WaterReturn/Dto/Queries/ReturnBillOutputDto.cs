using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillOutputDto
    {
        public BedBesOutputDto Bill { get; set; }
        public RepairOutputDto Repair { get; set; }
        public AutoBackOutputDto AutoBack { get; set; }
        public ReturnBillOutputDto(BedBesOutputDto bill, RepairOutputDto repair, AutoBackOutputDto autoBack)
        {
            Bill = bill;
            Repair = repair;
            AutoBack = autoBack;
        }
    }
}
