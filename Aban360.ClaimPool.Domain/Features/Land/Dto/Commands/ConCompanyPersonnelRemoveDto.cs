namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record ConCompanyPersonnelRemoveDto
    {
        public int CompanyId { get; set; }
        public int Index { get; set; }
        public Guid RemovedBy { get; set; }
        public DateTime RemovedDateTime { get; set; } = DateTime.Now;
        public ConCompanyPersonnelRemoveDto(int companyId, int index, Guid removedBy)
        {
            CompanyId = companyId;
            Index = index;
            RemovedBy = removedBy;
        }
        public ConCompanyPersonnelRemoveDto()
        {
        }
    }
}
