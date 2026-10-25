using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Implementations
{
    public sealed class BankReceiptParser : IBankReceiptParser
    {
        public void Parse(BankReceiptImport import, string bankCode, string bankIdentifier)
        {
            import.Rows.Clear();
            import.Issues.Clear();
            import.Header = null;
            import.DataRowCount = 0;
            // All fields specified on pages 14-16 are ASCII numeric, even in an ANSI file.
            if (import.Content.Any(b => b > 127 || b == 0))
            {
                import.Issues.Add(new("Encoding", "فایل باید متن ANSI با فیلدهای عددی لاتین باشد."));
                return;
            }
            var name = Regex.Match(import.FileName, @"^([A-Z]{3})([0-9]{8})([A-Z]{2})([0-9]{3})\.([0-9]{3})$");
            if (!name.Success)
                import.Issues.Add(new("FileName", "نام فایل با الگوی بانک، تاریخ، شرکت، توالی و کد تابعه مطابقت ندارد."));
            using var reader = new StringReader(Encoding.ASCII.GetString(import.Content));
            var header = reader.ReadLine();
            if (!Numeric(header, 32))
                import.Issues.Add(new("Header", "سطر اول باید شامل ۳۲ رقم باشد.", 1));
            else
            {
                import.Header = new()
                {
                    UtilityCode = header![..1], SubUtilityCode = header.Substring(1, 3),
                    BankCode = header.Substring(4, 2), SendDate = header.Substring(6, 8),
                    Amount = long.Parse(header.Substring(14, 10), CultureInfo.InvariantCulture) * 1000,
                    RecordCount = int.Parse(header.Substring(24, 8), CultureInfo.InvariantCulture)
                };
                if (!ValidDate(import.Header.SendDate))
                    import.Issues.Add(new("SendDate", "تاریخ ارسال معتبر نیست.", 1));
                if (import.Header.BankCode != bankCode.PadLeft(2, '0'))
                    import.Issues.Add(new("Bank", "کد بانک Header با بانک انتخاب‌شده مطابقت ندارد.", 1));
                if (import.Header.UtilityCode != "1")
                    import.Issues.Add(new("Utility", "فایل متعلق به آبفا نیست.", 1));
                if (name.Success)
                {
                    if (name.Groups[1].Value != bankIdentifier)
                        import.Issues.Add(new("BankIdentifier", "مشخصه بانک در نام فایل با بانک انتخاب‌شده مطابقت ندارد."));
                    if (name.Groups[2].Value != import.Header.SendDate)
                        import.Issues.Add(new("FileSendDate", "تاریخ نام فایل با تاریخ Header مطابقت ندارد."));
                    if (name.Groups[3].Value != "WA")
                        import.Issues.Add(new("FileUtility", "نوع شرکت در نام فایل متعلق به آبفا نیست."));
                    if (name.Groups[5].Value != import.Header.SubUtilityCode)
                        import.Issues.Add(new("SubUtilityCode", "کد شرکت تابعه در نام فایل با Header مطابقت ندارد."));
                }
            }
            var hashes = new HashSet<string>();
            string? line;
            int lineNumber = 1;
            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;
                import.DataRowCount++;
                if (!Numeric(line, 48))
                {
                    import.Issues.Add(new("RowStructure", "سطر پرداخت باید شامل ۴۸ رقم باشد.", lineNumber));
                    import.Rows.Add(new BankReceiptRow
                    {
                        LineNumber = lineNumber, RawData = line,
                        Issues = new() { new("RowStructure", "سطر پرداخت باید شامل ۴۸ رقم باشد.", lineNumber) }
                    });
                    continue;
                }
                var row = new BankReceiptRow
                {
                    LineNumber = lineNumber, RawData = line, BranchCode = line[..6],
                    ChannelType = short.Parse(line.Substring(6, 2), CultureInfo.InvariantCulture),
                    PayDate = line.Substring(8, 8), BillId = line.Substring(16, 13).TrimStart('0'),
                    PaymentId = line.Substring(29, 13).TrimStart('0'), ReferenceCode = line.Substring(42, 6)
                };
                row.Hash = Convert.ToHexString(SHA256.HashData(Encoding.ASCII.GetBytes(row.BillId + ":" + row.PaymentId)));
                if (!hashes.Add(row.Hash)) row.Issues.Add(new("DuplicateRow", "پرداخت در فایل تکرار شده است.", lineNumber));
                if (!ValidDate(row.PayDate)) row.Issues.Add(new("PayDate", "تاریخ پرداخت معتبر نیست.", lineNumber));
                if (!Enum.IsDefined(typeof(PaymentMethodEnum), row.ChannelType)) row.Issues.Add(new("Channel", "روش پرداخت معتبر نیست.", lineNumber));
                if (!TransactionIdGenerator.IsBillIdStructureValid(row.BillId)) row.Issues.Add(new("BillId", "ساختار شناسه قبض معتبر نیست.", lineNumber));
                if (row.BillId.Length < 2 || row.BillId[^2] != '1') row.Issues.Add(new("NotWater", "پرداخت متعلق به آبفا نیست.", lineNumber));
                if (row.PaymentId.Length < 6 || row.PaymentId[^2].ToString() != CheckDigit(row.PaymentId[..^2])
                    || row.PaymentId[^1].ToString() != CheckDigit(row.BillId + row.PaymentId[..^1]))
                    row.Issues.Add(new("PaymentId", "شناسه پرداخت معتبر نیست.", lineNumber));
                if (row.PaymentId.Length >= 6)
                    row.Amount = long.Parse(row.PaymentId[..^5], CultureInfo.InvariantCulture) * 1000;
                if (row.Amount <= 0) row.Issues.Add(new("Amount", "مبلغ پرداخت باید مثبت باشد.", lineNumber));
                import.Rows.Add(row);
            }
            if (import.Header != null)
            {
                if (import.Header.RecordCount != lineNumber - 1 || import.Rows.Count != lineNumber - 1 || import.Rows.Count == 0)
                    import.Issues.Add(new("Count", "تعداد پرداخت‌های معتبر با Header مطابقت ندارد یا فایل خالی است."));
                if (import.Header.Amount != import.Rows.Sum(r => r.Amount))
                    import.Issues.Add(new("Total", "جمع مبلغ سطرها با مبلغ Header مطابقت ندارد."));
            }
        }

        private static bool Numeric(string? value, int length) => value?.Length == length && value.All(c => c >= '0' && c <= '9');
        private static bool ValidDate(string value)
        {
            try
            {
                var calendar = new PersianCalendar();
                calendar.ToDateTime(int.Parse(value[..4]), int.Parse(value.Substring(4, 2)), int.Parse(value.Substring(6, 2)), 0, 0, 0, 0);
                return true;
            }
            catch (ArgumentException) { return false; }
        }
        private static string CheckDigit(string value)
        {
            int sum = 0, weight = 2;
            for (int i = value.Length - 1; i >= 0; i--)
            {
                sum += (value[i] - '0') * weight;
                weight = weight == 7 ? 2 : weight + 1;
            }
            int remainder = sum % 11;
            return (remainder < 2 ? 0 : 11 - remainder).ToString(CultureInfo.InvariantCulture);
        }
    }
}
