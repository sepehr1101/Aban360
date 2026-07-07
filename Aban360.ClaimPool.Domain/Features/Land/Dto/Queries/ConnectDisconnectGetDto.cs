namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record ConnectDisconnectGetDto
    {
        public long Id { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string BillId { get; set; }
        public long WaterDebt { get; set; }
        public DateTime CommandDateTime { get; set; }
        public Guid CommandBy { get; set; }
        public int CommandCauseId { get; set; }
        public string CommandCauseTitle { get; set; }
        public DateTime? ResultDateTime { get; set; }
        public Guid? ResultBy { get; set; }
        public int? ResultId { get; set; }
        public string? ResultTitle { get; set; }
        public int MeterDiameterId { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string? CompanyTitle { get; set; }
        public int? CompanyId { get; set; }
        public Guid? PersonnelId { get; set; }
        public string? PersonnelName { get; set; }
        public string? JudicialNoticeId { get; set; }
        public int TypeId { get; set; }
        public string TypeTitle { get; set; }
        public string? Description { get; set; }
        public DateTime? RemovedDateTime { get; set; }
        public Guid? RemovedBy { get; set; }


    }
}
