using Aban360.PaymentPool.Domain.Constansts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

[Table(nameof(Credit))]
public class Credit
{
    public long Id { get; set; }

    public string BillId { get; set; } = null!;

    public string PaymentId { get; set; } = null!;

    public long InvoiceId { get; set; }

    public long InvoiceInstallmentId { get; set; }

    public long Amount { get; set; }

    public int UploaderId { get; set; }

    public CreditorTypeEnum CreditorTypeId { get; set; }

    public PaymentMethodEnum PaymentMethodId { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string InsertLogInfo { get; set; } = null!;

    public string? RemoveLogInfo { get; set; }

    public string Hash { get; set; } = null!;

    public virtual Uploader Uploader { get; set; } = null!;
}
