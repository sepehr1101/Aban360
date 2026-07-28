namespace Aban360.Common.BaseEntities
{
    public record ZoneIdAndCustomerNumberAndBillId
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public ZoneIdAndCustomerNumberAndBillId(int zoneId, int customerNumber, string billId)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            BillId = billId;
        }
        public ZoneIdAndCustomerNumberAndBillId()
        {

        }
    }
}
