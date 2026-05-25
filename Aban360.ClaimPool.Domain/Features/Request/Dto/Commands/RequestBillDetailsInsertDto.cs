namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record RequestBillDetailsInsertDto
    {
        public string TrackNumber { get; set; }   
        public int ZoneId { get; set; }                
        public int CustomerNumber { get; set; }        
        public string TypeId { get; set; }        
        public int ItemId { get; set; }                
        public string ItemTitle { get; set; }      
        public long Amount { get; set; }               
        public long OffAmount { get; set; }            
        public string OffTitle { get; set; }         
        public long FinalAmount { get; set; }          
        public string RegisterDate { get; set; }     
        public string? ZoneTitle { get; set; }
        public string BillId { get; set; }
        public int DomesticCount { get; set; }
        public int CommercialCount { get; set; }
        public int OtherCount { get; set; }
        public int ContractualCapacity { get; set; }
        public int? TypeCode { get; set; }      
        public int? UsageId { get; set; }          
        public string? UsageTitle { get; set; }       
        public string? PayId { get; set; }
    }
}
