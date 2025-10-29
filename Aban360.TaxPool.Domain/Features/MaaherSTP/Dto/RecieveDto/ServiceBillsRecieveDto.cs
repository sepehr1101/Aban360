namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto
{
    public record ServiceBillsRecieveDto
    {
        public string Uuid { get; set; }
        public string Message { get; set; }
        public int Error_code { get; set; }
    }
    public record InquiryRecieveDto
    {
        public string Uuid { get; set; }
        public string Status { get; set; }//todo : save statusList in database
        public string Error_message { get; set; }
        public ErrorsRecieveDto Errors { get; set; }
        public Guid Reference_number { get; set; }
        public Guid Confirm_number { get; set; }
        public ErrorsRecieveDto Tax_errors { get; set; }
    }
    public record ErrorsRecieveDto
    {
        public string Key { get; set; }
        public string ErrorKeyPosition { get; set; }
        public int ErrorKeyIndex { get; set; }
        public string Message { get; set; }
    }
    public record TaxErrorsRecieveDto
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }//todo: check datatype
    }
}
