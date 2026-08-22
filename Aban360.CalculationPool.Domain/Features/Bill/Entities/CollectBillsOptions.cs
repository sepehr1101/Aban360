namespace Aban360.CalculationPool.Domain.Features.Bill.Entities
{
    public record CollectBillsOptions
    {
        public const string SectionName = "CollectBills";
        public string BaseUrl { get; set; } = default!;
        public string UserName{ get; set; } = default!;
        public string Password{ get; set; } = default!;
        public string Login { get; set; } = default!;
        public string SubscriptionsInfo { get; set; } = default!;
        public string Upload { get; set; } = default!;
        public string AssingUploadedFile { get; set; } = default!;
        public string GetFileDetail { get; set; } = default!;
        public string ConfirmFile { get; set; } = default!;
        public string SubscriptionByBillId { get; set; } = default!;
    }
}
