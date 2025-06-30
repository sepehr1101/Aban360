namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record ChangeMainOutputDto
    {
        public string ChangeDate { get; set; }
        public List<string> ChangeDetail{ get; set; }=new List<string>();
    }
}
