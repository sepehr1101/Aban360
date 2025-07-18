﻿namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record WaterPaymentReceivableHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }

        public int SumTotalCount { get; set; }
        public int SumOverdueCount { get; set; }
        public long SumOverdueAmount { get; set; }
        public long SumCurrentAmount { get; set; }
        public long SumTotalAmount{ get; set; }


    }
}
