namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record SpecialConditionTagsInfoDto
    {
        public string HandoverTitle { get; set; }
        public string CoverageStatus { get; set; }
        public string UsageStateTitle { get; set; }

        public bool SpecialBranch { get; set; }
        public short VacantUnitCount { get; set; }
        public short HouseholderNumber { get; set; }

        public bool NonSequentialFlag { get; set; }

        #region Tag
        public IEnumerable<WaterMeterTagInfoDto> WaterMeterTags { get; set; }
        #endregion

    }
    public record WaterMeterTagInfoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}