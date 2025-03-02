using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(Usage), Schema = TableSchema.Name)]
public class Usage
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ProvinceId { get; set; }
    
}
