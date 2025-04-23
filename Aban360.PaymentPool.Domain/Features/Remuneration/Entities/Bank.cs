using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.PaymentPool.Domain.Features.Remuneration.Entities;

[Table(nameof(Bank))]
public class Bank
{
    public short Id { get; set; }

    public string BankName { get; set; } = null!;

    public string? Icon { get; set; }

    public string CentralBankCode { get; set; } = null!;

    public string? Description { get; set; }
}
