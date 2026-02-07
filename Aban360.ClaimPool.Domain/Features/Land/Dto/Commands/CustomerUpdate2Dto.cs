namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record CustomerUpdate2Dto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }

        public int BranchTypeId { get; set; }
        public int DeletionStateId { get; set; }
        public bool IsSpecial { get; set; }
        public int EmptyUnit { get; set; }
        public int HouseholdNumber { get; set; }

    }
}
