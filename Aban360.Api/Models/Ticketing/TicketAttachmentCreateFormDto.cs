using System.ComponentModel.DataAnnotations;

namespace Aban360.Api.Models.Ticketing
{
    public class TicketAttachmentCreateFormDto
    {
        [Range(1, long.MaxValue)]
        public long TicketMessageId { get; set; }
        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
