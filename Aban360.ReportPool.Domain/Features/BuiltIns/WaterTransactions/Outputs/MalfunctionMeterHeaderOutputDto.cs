﻿namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record MalfunctionMeterHeaderOutputDto
    {
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public long TotalPayable { get; set; }
    }
}
