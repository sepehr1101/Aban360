using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities
{
    [Table(nameof(EstateBoundType))]
    public class EstateBoundType
    {
        public short Id { get; set; }
        public string Title { get; set; } = default!;
    }
}
