namespace Aban360.ReportPool.Domain.Features.Tagging
{
    public class TagGroupDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreateDateTime { get; set; }
        public DateTime? DeleteDateTime { get; set; }
    }

    public class CreateTagGroupDto
    {
        public string Title { get; set; } = string.Empty;
    }

    public class UpdateTagGroupDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
    public record TagsHeaderOutputDto
    {
        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; }

        public int Count { get; set; }
        public int CustomerCount { get; set; }
        public string Title { get; set; }
    }
    public record TagsInputDto
    {
        public ICollection<int> TagIds { get; set; }
    }

    public record TagsReportSummaryDataOutputDto
    {
        public string TagsTitle { get; set; }
        public string ItemTitle { get; set; }
        public int Count { get; set; }
        public int CustomerCount { get; set; }
    }

    public record TagGroupReportDetailDataOutputDto
    {
        public string TagGroupTitle { get; set; }
        public int TagGroupId { get; set; }
        public string TagTitle { get; set; }
        public int TagId { get; set; }
        public string BillIdTagsExpireDateJalali { get; set; }
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string UsageTitle { get; set; }
        public string BranchType { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string SiphonDiameterTitle { get; set; }
        public string Address { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public int ContractualCapacity { get; set; }
        public string BillId { get; set; }
        public int EmptyUnit { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string RegionTitle { get; set; }
        public string NationalCode { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FatherName { get; set; }
    }
}
