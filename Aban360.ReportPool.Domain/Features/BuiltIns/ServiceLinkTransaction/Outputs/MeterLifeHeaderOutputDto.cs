namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record MeterLifeHeaderOutputDto
    {
        public int FromLifeInDay { get; set; }
        public int ToLifeInDay { get; set; }

        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string Title { get; set; }

        public int AverageLifeInDay { get; set; }
        public string AverageLifeText { get; set; }
        
        public int MaxLifeInDay { get; set; }
        public string MaxLifeText { get; set; }
        
        public int MinLifeInDay { get; set; }
        public string MinLifeText { get; set; }


    }
}
