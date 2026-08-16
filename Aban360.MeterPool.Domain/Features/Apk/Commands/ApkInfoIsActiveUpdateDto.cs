namespace Aban360.MeterPool.Domain.Features.Apk.Commands
{
    public record ApkInfoIsActiveUpdateDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public ApkInfoIsActiveUpdateDto(int id, bool isActive)
        {
            Id = id;
            IsActive = isActive;
        }
        public ApkInfoIsActiveUpdateDto()
        {
        }
    }
}
