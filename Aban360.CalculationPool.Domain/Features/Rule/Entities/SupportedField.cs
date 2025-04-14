using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Rule.Entities
{
    [Table(nameof(SupportedField))]
    public class SupportedField
    {
        public short Id { get; set; }
        public string Title { get; set; } = default!;
        public string Desciption { get; set; } = default!;
    }
}
