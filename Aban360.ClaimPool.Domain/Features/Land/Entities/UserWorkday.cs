using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(UserWorkday))]
public class UserWorkday
{
    public short Id { get; set; }

    public Guid UserId { get; set; }

    public string UserFullname { get; set; } = null!;

    public string FromReadingNumber { get; set; } = null!;

    public string ToReadingNumber { get; set; } = null!;

    public string DateJalali { get; set; } = null!;

    public int ZoneId { get; set; }

    public string ZoneTitle { get; set; } = null!;
}
