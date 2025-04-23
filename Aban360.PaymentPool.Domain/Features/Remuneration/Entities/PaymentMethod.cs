using Aban360.PaymentPool.Domain.Constansts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.PaymentPool.Domain.Features.Remuneration.Entities;

[Table(nameof(PaymentMethod))]
public class PaymentMethod
{
    public PaymentMethodEnum Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Icon { get; set; }

    public string? Description { get; set; }
}
