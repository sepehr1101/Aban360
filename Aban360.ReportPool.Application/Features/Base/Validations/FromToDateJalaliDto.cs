namespace Aban360.ReportPool.Application.Features.Base.Validations
{
    public record FromToDateJalaliDto
    {
        public string FromDateJalai { get; set; }
        public string ToDateJalai { get; set; }
        public FromToDateJalaliDto(string fromDate, string toDate)
        {
            FromDateJalai = fromDate;
            ToDateJalai = toDate;
        }
    }
}
