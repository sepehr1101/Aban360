namespace Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries
{
    public record NerkhGetDto
    {
        public int Id { get; set; }
        public string? Date1 { get; set; }
        public string? Date2 { get; set; }
        public float Ebt { get; set; }
        public float Ent { get; set; }
        public string? Vaj { get; set; }
        public string? CalcVaj { get; set; }
        public int Cod { get; set; }
        public int Olgo { get; set; }
        public string? Desc { get; set; }
        public string? OVaj { get; set; }
        public string? OVajFaz { get; set; }
        public string? VajFaz { get; set; }
        public int Bodjeh_new { get; set; }
        public bool ZaribTadil { get; set; }
        public bool Tabsare2 { get; set; }
        public bool ZaribFasl { get; set; }
        public int ZaribBodje { get; set; }

        public int Duration { get; set; }
        public double PartialConsumption { get; set; }
        public double DailyAverageConsumption { get; set; }

        public int C { get; set; }

        //not mapped from db
        public double Multiplier { get; set; }

        //public void SetDate1(string date1)
        //{
        //    Date1=date1;
        //}
        //public void SetDate2(string date2)
        //{
        //    Date2 = date2;
        //}
        //public void SetDailyAverageConsumption(double dailyAverageConsumption)
        //{
        //    DailyAverageConsumption = dailyAverageConsumption;
        //}
    }
}
