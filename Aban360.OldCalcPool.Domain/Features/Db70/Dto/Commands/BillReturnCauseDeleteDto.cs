using System.Text.Json.Serialization;

namespace Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands
{
    public record BillReturnCauseDeleteDto
    {
        public short Id { get; set; }
        [JsonIgnore]
        public DateTime RemoveDateTime { get; set; }
        [JsonIgnore]
        public Guid RemoveByUserId { get; set; }
    }
}
