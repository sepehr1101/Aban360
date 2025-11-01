namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto
{
    public static class MaaherTSPUri
    {
        public static string SandboxUrl { get { return @"https://thirdparty-api-sandbox.maahertsp.com"; } }
        public static string MaaherUrl { get { return @"https://v4-invogate.maahertsp.com"; } }
        public static string SentInvoiceUrl(string tins, string fiscalId, string authentication) => @$"/api/v1/invoices?tins={tins}&fiscalId={fiscalId}&AuthenticationId={authentication}";
    }
}