﻿using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

[Table(nameof(BankAccount))]
public class BankAccount
{
    public short Id { get; set; }

    public short BankId { get; set; }

    public string Title { get; set; } = null!;
    
    public string IBan { get; set; } = null!;
    
    public string Number { get; set; } = null!;

    public AccountTypeEnum AccountTypeId { get; set; }

    public int RegionId { get; set; }

    public string RegionTitle { get; set; } = null!;

    public string? Icon { get; set; }

    public virtual Bank Bank { get; set; } = null!;
    public virtual AccountType AccountType { get; set; }
}
