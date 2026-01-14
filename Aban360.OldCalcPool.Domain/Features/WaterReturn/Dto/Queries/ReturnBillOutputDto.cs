using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillOutputDto
    {   
        public ReturnBillDataOutputDto Bill { get; set; }
        public ReturnBillDataOutputDto Repair { get; set; }
        public ReturnBillDataOutputDto AutoBack { get; set; }
        public ReturnBillOutputDto(ReturnBillDataOutputDto bill, ReturnBillDataOutputDto repair, ReturnBillDataOutputDto autoBack)
        {
            Bill = bill;
            Repair = repair;
            AutoBack = autoBack;
        }
    }
}
