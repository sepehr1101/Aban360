namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record ChangeMainInfoDto
    {
        public string ChangeTypeTitle { get; set; }
        public string LastState { get; set; }
        public string CurrentState { get; set; }
        public string ChangeDate { get; set; }
        public string SystemUserCode { get; set; }

    }
}
