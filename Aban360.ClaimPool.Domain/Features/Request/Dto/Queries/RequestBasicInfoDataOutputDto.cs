using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record RequestBasicInfoDataOutputDto
    {
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string? FatherName { get; set; }
        public int BranchTypeId { get; set; }
        public string? BranchTypeTitle { get; set; }
        public string CustomerNumber { get; set; }
        public string? ReadingNumber { get; set; }
        public int TrackNumber { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? NationalCode { get; set; }
        public string? Description { get; set; }
        public string RequestDateJalali { get; set; }
        public string? RegisterDateJalali { get; set; }
        public int DiscountCount { get; set; }
        public int DiscountTypeId { get; set; }
        public string? DiscountTypeTitle { get; set; }
        public string? PhoneNumber { get; set; }
        public string? NeighbourBillId { get; set; }
        public int ServiceGroupId { get; set; }
        public string ServiceGroupTitle { get; set; }
        public int PreviousStatusId { get; set; }
        public string PreviousStatusTitle { get; set; }
        public string PreviousStepDateJalali { get; set; }
        public string BillId { get; set; }
        public Guid PreviousTrackId { get; set; }
        public IEnumerable<SelectionDto> ServiceSelected { get; set; }
    }
}