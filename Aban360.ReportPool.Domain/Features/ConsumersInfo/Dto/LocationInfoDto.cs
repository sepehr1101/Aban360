namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record LocationInfoDto
    {
        public string AccountNumber { get; set; }
        public string PostalCode { get; set; }
        public string X { get; set; }
        public string Y { get; set; }

        public string EvaluatorSpecifications { get; set; } = null!;
        public string Address { get; set; } = null!;


    }
}
