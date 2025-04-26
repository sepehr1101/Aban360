using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

[Table(nameof(BankAccount))]
public class BankAccount
{
    public short Id { get; set; }

    public short BankId { get; set; }

    public string Title { get; set; } = null!;

    public short AccountTypeId { get; set; }

    public int ZoneId { get; set; }

    public string ZoneTitle { get; set; } = null!;

    public virtual Bank Bank { get; set; } = null!;
}
