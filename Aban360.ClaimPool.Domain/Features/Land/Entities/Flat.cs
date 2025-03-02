using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(Flat), Schema = TableSchema.Name)]
public class Flat
{
    public int Id { get; set; }

    public int EstateId { get; set; }

    public string? PostalCode { get; set; }

    public short Storey { get; set; }

    public string? Description { get; set; }

    public virtual Estate Estate { get; set; } = null!;
}
