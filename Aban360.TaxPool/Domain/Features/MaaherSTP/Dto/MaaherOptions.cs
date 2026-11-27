namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto
{
    public class MaaherOptions
    {
        //public static string SandboxUrl { get { return @"https://thirdparty-api-sandbox.maahertsp.com"; } }
        //public static string MaaherUrl { get { return @"https://v4-invogate.maahertsp.com"; } }
        //public static string SentInvoiceUrl(string tins, string fiscalId, string authentication) => @$"/api/v1/invoices?tins={tins}&fiscalId={fiscalId}&AuthenticationId={authentication}";

        public const string SectionName = "Tax";

        public string MaaherBaseUrl { get; set; } = default!;
        public string SentInvoiceUrl { get; set; } = default!;
        public string Tins { get; set; } = default!;
        public string FiscalId { get; set; } = default!;
        public string AuthenticationId { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}