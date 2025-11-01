namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto
{
    public record SentInvoiceRecieveDto
    {
        public ICollection<_ErrorsRecieveDto> Errors { get; set; }
       
        public int ResultCode { get; set; }
        public int?  StatusCode { get; set; }
        public string? Description { get; set; }
      
        public string Status { get; set; }
        public bool Success { get; set; }
        public string TaxId { get; set; }
        public string Uid { get; set; }
    }
    public record _ErrorsRecieveDto
    {
        public string Field { get; set; }
        public int Index { get; set; }
        public string Message { get; set; }
        public string Property { get; set; }
        public string Tag { get; set; }
        public string Value { get; set; }
    }
    public record MaaherErrorsDto
    {
        public short Id { get; set; }
        public int ErrorCode { get; set; }
        public short HttpStatus { get; set; }
        public string Description { get; set; }
    }
}