using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.TimeTable.Entites;

[Table(nameof(OfficialHoliday))]
public class OfficialHoliday
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string DateJalali { get; set; } = null!;
}
