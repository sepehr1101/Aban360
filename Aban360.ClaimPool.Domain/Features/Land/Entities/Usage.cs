using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(Usage))]
public class Usage
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ProvienceId { get; set; }
}
