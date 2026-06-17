namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record KartRemoveByConditionDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public long Amount { get; set; }
        public string RegisterDateJalali { get; set; }
        public int TypeCode { get; set; }
        public int ItemId { get; set; }
        public KartRemoveByConditionDto(int zoneId,int customerNumber,long amount,string registerDateJalali,int typeCode,int itemId)
        {
            ZoneId=zoneId;
            CustomerNumber=customerNumber;
            Amount=amount;
            RegisterDateJalali=registerDateJalali;  
            TypeCode=typeCode;
            ItemId=itemId;
        }
        public KartRemoveByConditionDto()
        {
        }
    }
}
