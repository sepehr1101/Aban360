using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Rule.Entities
{
    [Table(nameof(TariffByDetail))]
    public class TariffByDetail
    {
        public int Id { get; set; }
        public string FromDateJalali { get; set; } = null!;
        public string ToDateJalali { get; set; } = null!;
        public short UsageId { get; set; }
        public short IndividualTypeId { get; set; }//Todo: Prop Name
        public string FromBillId { get; set; } = null!;
        public string ToBillId { get; set; } = null!;
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; } = null!;
        public string? Description { get; set; }
    }
}
