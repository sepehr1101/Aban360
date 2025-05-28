namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record SpecialConditionTagsInfoDto
    {
        public string HandoverTitle { get; set; }
        public string DiscountTypeTitle { get; set; }
        public string UsageStateTitle { get; set; }

        public bool SpecialBranch { get; set; }
        public short EmptyUnit { get; set; }
        public short HouseholderNumber { get; set; }

        public bool NonSequentialFlag { get; set; }

        #region Tag
        public IEnumerable<TagsInfoDto>  tagsInfoDtos{ get; set; }
        #endregion

    }
    public record TagsInfoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}