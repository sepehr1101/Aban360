﻿namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record UnspecifiedPaymentInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string FromAmount { get; set; }
        public string ToAmount { get; set; }

    }
}

