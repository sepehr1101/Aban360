using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillOutputDto
    {
        public IEnumerable<BedBesCreateDto> Bills { get; set; }
        public RepairCreateDto Repair { get; set; }
        public AutoBackCreateDto AutoBack { get; set; }
        public ReturnBillOutputDto(IEnumerable<BedBesCreateDto> bills, RepairCreateDto repair, AutoBackCreateDto autoBack)
        {
            Bills = bills;
            Repair = repair;
            AutoBack = autoBack;
        }
    }
}
