using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record BillIssueRemainedOutputDto
    {
        public int CustomerNumber { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string BillId { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string FullName { get; set; }
        public string ReadingNumber { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int BranchTypeId { get; set; }
        public string BranchTypeTitle { get; set; }
        public int ContractualCapacity { get; set; }
        public long Amount { get; set; }
        public string PaymentId { get; set; }
        public string CurrentDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
    }
}
