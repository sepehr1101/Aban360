using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities
{
    [Table(nameof(EstateBoundType), Schema = TableSchema.Name)]
    public class EstateBoundType
    {
        public short Id { get; set; }
        public string Title { get; set; } = default!;
    }
}
