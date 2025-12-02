using System.Text.Json.Serialization;

namespace Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands
{
    public record SCreateDto
    {
        public int ZoneId { get; set; }
        public int Olgo { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
