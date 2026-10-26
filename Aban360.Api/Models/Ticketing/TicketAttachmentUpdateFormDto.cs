using System.ComponentModel.DataAnnotations;

namespace Aban360.Api.Models.Ticketing
{
    public class TicketAttachmentUpdateFormDto
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }
        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
