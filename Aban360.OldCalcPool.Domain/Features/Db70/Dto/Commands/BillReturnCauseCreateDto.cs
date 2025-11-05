using System.Text.Json.Serialization;

namespace Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands
{
    public record BillReturnCauseCreateDto
    {
        public int Code { get; set; }
        public string Title { get; set; }
        [JsonIgnore]
        public DateTime RegisterDateTime { get; set; }
        [JsonIgnore]
        public Guid RegisterByUserId { get; set; }
    }
}
