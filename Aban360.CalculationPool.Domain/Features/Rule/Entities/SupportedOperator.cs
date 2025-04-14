using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Rule.Entities
{
    [Table(nameof(SupportedOperator))]
    public class SupportedOperator
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
