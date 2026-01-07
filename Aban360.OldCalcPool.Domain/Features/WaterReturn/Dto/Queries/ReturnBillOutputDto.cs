using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillOutputDto
    {
        public ReturBillDataOutputDto Bill { get; set; }
        public ReturBillDataOutputDto Repair { get; set; }
        public ReturBillDataOutputDto AutoBack { get; set; }
        public ReturnBillOutputDto(ReturBillDataOutputDto bill, ReturBillDataOutputDto repair, ReturBillDataOutputDto autoBack)
        {
            Bill = bill;
            Repair = repair;
            AutoBack = autoBack;
        }
    }
}
