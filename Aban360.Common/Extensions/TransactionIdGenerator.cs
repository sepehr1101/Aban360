namespace Aban360.Common.Extensions
{
    /// <summary>
    /// تولید و صحت سنجی شناسه قبض، شناسه پرداخت و کد پیگیری
    /// </summary>
    public static class TransactionIdGenerator
    {
        private const int _minLength = 6;
        private const int _maxLength = 13;
        private const int _abfaServiceCode = 1;

        /// <summary>
        /// تولید شناسه قبض 
        /// </summary>
        /// <param name="customerNumber">شماره پرونده یا ردیف</param>
        /// <param name="_3digitZoneId">کد شهر سه رقمی</param>
        /// <returns></returns>
        public static string GenerateBillId(string customerNumber, string _3digitZoneId)
        {           
            var zoneIdAndServiceProvider = _3digitZoneId + Convert.ToInt32(_abfaServiceCode);
            var billId = CheckDigitManager.GetAppendedCheckDigitString(customerNumber + zoneIdAndServiceProvider);
            return billId;
        }

        /// <summary>
        /// تولید شناسه پرداخت
        /// </summary>
        /// <param name="amount">مبلغ</param>
        /// <param name="billId">شناسه قبض</param>
        /// <param name="yMM">فرمت سال و ماه- اختیاری</param>
        /// <returns></returns>
        public static string GeneratePaymentId(long amount, string billId, string yMM = "000")
        {
            string amountChanged = GetAmount(amount);
            string firstStepPayId = CheckDigitManager.GetAppendedCheckDigitString(amountChanged + yMM);
            string finalCheckDigit = CheckDigitManager.GenerateCheckDigit(billId + firstStepPayId);
            return firstStepPayId + finalCheckDigit;
        }
        public static bool IsBillIdStructureValid(string billId)
        {
            string checkDigit, billIdLastChar, remindedChars, billIdFirstChar;
            int billIdCount;
            if (string.IsNullOrWhiteSpace(billId))
            {
                return false;
            }
            billId = billId.Trim();
            billIdFirstChar = billId.Substring(0, 1);
            billIdCount = billId.Length;
            if (billIdCount > _maxLength || billIdCount < _minLength || billIdFirstChar == "0")
            {
                return false;
            }
            foreach (char c in billId)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            billIdLastChar = billId.Substring(billId.Length - 1, 1);
            remindedChars = billId.Substring(0, billId.Length - 1);
            checkDigit = CheckDigitManager.GenerateCheckDigit(remindedChars);
            if (billIdLastChar != checkDigit)
            {
                return false;
            }
            return true;
        }
        private static string GetAmount(long amount)
        {
            var mod = amount % 1000;
            var devidedAmount = amount / 1000;
            var changedAmount = mod > 500 ? (devidedAmount + 1) : devidedAmount;
            return changedAmount.ToString();
        }
    }
    file static class CheckDigitManager
    {
        const int _zero = 0;
        const int _one = 1;
        const int _two = 2;
        const int _seven = 7;
        const int _eleven = 11;

        /// <summary>
        ///     تولید کد دیجیت و سپس الحاق به رشته عددی اصلی
        /// </summary>
        /// <param name="originNumber">رشته عددی اصلی</param>
        /// <returns></returns>
        internal static string GetAppendedCheckDigitString(string originNumber)
        {
            var checkDigit = GenerateCheckDigit(originNumber);
            return string.Concat(originNumber, checkDigit);
        }
        internal static string GenerateCheckDigit(string originNumber)
        {

            int i = _one, j = _one, temp = _zero, digitCode;
            do
            {
                j = j + _one;
                if (j > _seven)
                    j = _two;
                temp = temp + Convert.ToInt32(originNumber.Substring(originNumber.Length - i, _one)) * j;
                i++;
            } while (originNumber.Length >= i);

            digitCode = temp % _eleven;
            digitCode = digitCode < _two ? _zero : _eleven - digitCode;
            return digitCode.ToString().Trim();
        }
    }
}
