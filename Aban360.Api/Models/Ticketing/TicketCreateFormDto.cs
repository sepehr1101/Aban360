using System.ComponentModel.DataAnnotations;

namespace Aban360.Api.Models.Ticketing
{
    public class TicketCreateFormDto
    {
        [Required]
        public string Subject { get; set; } = null!;
        public string? Body { get; set; }
        public ICollection<IFormFile> Attachments { get; set; } = new List<IFormFile>();
    }
}
