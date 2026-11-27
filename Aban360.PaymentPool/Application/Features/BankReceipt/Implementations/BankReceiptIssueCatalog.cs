using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Implementations
{
    internal static class BankReceiptIssueCatalog
    {
        public static BankReceiptIssue Normalize(BankReceiptIssue issue, int lineNumber)
        {
            string? message = issue.Code switch
            {
                "UnknownCityCode" or "CityNotFound" => "کد شهر شناسه قبض شناسایی نشد.",
                "SubUtilityCode" or "InvalidSubUtilityCode" => "کد شرکت تابعه معتبر نیست یا با مشخصات فایل مطابقت ندارد.",
                "UnknownBillId" => "شناسه قبض در اطلاعات مشترکین شناسایی نشد.",
                "OutsideIsfahan" => "کد شهر مربوط به شهرهای آبفا اصفهان نیست.",
                "InvoiceMismatch" => "اطلاعات پرداخت با صورتحساب یا قسط مطابقت ندارد.",
                "HistoricalDuplicate" => "این وصولی در اطلاعات تاریخی قبلاً ثبت شده است.",
                _ => null
            };
            if (message == null)
                message = !string.IsNullOrWhiteSpace(issue.Message) && issue.Message.Any(c => c >= '\u0600' && c <= '\u06ff')
                    ? issue.Message : "وصولی به دلیل مغایرت در اطلاعات از فهرست قابل ثبت خارج شده است.";
            return issue with { Message = message, LineNumber = lineNumber };
        }
    }
}
