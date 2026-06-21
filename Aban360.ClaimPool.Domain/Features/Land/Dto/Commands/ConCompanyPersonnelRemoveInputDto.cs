namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record ConCompanyPersonnelRemoveInputDto
    {
        public int CompanyId { get; set; }
        public Guid Id { get; set; }
    }
}
