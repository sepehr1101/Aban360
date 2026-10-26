using System.ComponentModel.DataAnnotations;

namespace Aban360.Api.Models.Ticketing
{
    public class TicketMessageCreateFormDto
    {
        [Range(1, long.MaxValue)]
        public long TicketId { get; set; }
        public string? Body { get; set; }
        public ICollection<IFormFile> Attachments { get; set; } = new List<IFormFile>();
    }
}
