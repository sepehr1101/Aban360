namespace Aban360.Common.BaseEntities
{
    public record SubscriptionCardexInput
    {
        public string Input { get; set; } = default!;
        public string? FromDateJalali { get; set; }
        public bool HasRemovedBills { get; set; }
        public SubscriptionCardexInput(string input, string? fromDateJalali, bool hasRemovedBills)
        {
            Input = input;
            FromDateJalali = fromDateJalali;
        }
        public SubscriptionCardexInput()
        {

        }
    }
}
