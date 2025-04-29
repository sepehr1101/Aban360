using Aban360.PaymentPool.Domain.Constansts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

[Table(nameof(AccountType))]
public class AccountType
{
    public AccountTypeEnum Id { get; set; }

    public string Title { get; set; } = null!;
}
