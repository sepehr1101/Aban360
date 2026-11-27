using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

[Table(nameof(Uploader))]
public partial class Uploader
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public short BankId { get; set; }

    public Guid? DocumentId { get; set; }

    public string? ReferenceNumber { get; set; }
                                        
    public DateTime InsertDateTime { get; set; }

    public int InsertRecordCount { get; set; }

    public long Amount { get; set; }

    public virtual Bank Bank { get; set; } = null!;

    public virtual ICollection<Credit> Credits { get; set; } = new List<Credit>();
}
