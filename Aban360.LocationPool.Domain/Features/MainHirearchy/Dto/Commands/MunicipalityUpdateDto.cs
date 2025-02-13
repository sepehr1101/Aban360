namespace Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands
{
    public record MunicipalityUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int ZoneId { get; set; }
        public bool IsVillage { get; set; }
    }
}
