using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

[Table(nameof(BankFileStructure))]
public class BankFileStructure
{
    public short Id { get; set; }

    public short FromIndex { get; set; }

    public short ToIndex { get; set; }

    public short StringLenght { get; set; }

    public string Title { get; set; } = null!;

    public bool IsHeader { get; set; } = false;

    public short BankId { get; set; }

    public virtual Bank Bank { get; set; }
}
