﻿namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record WaterMeterReplacementsHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
    }
}
