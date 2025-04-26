using Aban360.PaymentPool.Domain.Constansts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

[Table(nameof(CreditorType))]
public class CreditorType
{
    public CreditorTypeEnum Id { get; set; }

    public string Title { get; set; } = null!;

}
