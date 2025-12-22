using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillOutputDto
    {
        public BedBesCreateDto Bill { get; set; }
        public RepairCreateDto Repair { get; set; }
        public AutoBackCreateDto AutoBack { get; set; }
        public ReturnBillOutputDto(BedBesCreateDto bill, RepairCreateDto repair, AutoBackCreateDto autoBack)
        {
            Bill = bill;
            Repair = repair;
            AutoBack = autoBack;
        }
    }
}
