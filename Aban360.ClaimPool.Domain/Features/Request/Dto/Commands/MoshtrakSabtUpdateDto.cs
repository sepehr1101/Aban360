using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record MoshtrakSabtUpdateDto
    {
        public int Id { get; set; }
        public bool IsRegister { get; set; }
        public string RegisterDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string? Description { get; set; }
        public MoshtrakSabtUpdateDto(int id, bool isRegister,string? description)
        {
            Id = id;
            IsRegister = isRegister;
            Description = description;
        }
        public MoshtrakSabtUpdateDto()
        {
        }
    }
}
