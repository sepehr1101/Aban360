using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Request.Entities;

[Table(nameof(Geteway))]
public class Geteway
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;
}
