using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities
{
    [Table(nameof(Handover))]
    public class Handover
    {
        public short Id { get; set; }

        public string Title { get; set; } = null!;
    }
}
