using System.Text.RegularExpressions;

namespace Aban360.UserPool.Application.Common.Base
{
    public static class ValidationAnsiString
    {
        public static bool ValidateAnsi(string input)
        {
            return input.All(x => x <= 255);
        }

        public static bool CheckTime(string input)//by format 00:00
        {
            string pattern = @"^([0-1][0-9]|2[0-3])\:([0-5][0-9])$";
            return Regex.IsMatch(input, pattern);
        }
        
        public static bool CheckPersianPhoneNumber(string input)//by format 09{9}
        {
            string pattern = "^09[0-9]{9}$";
            return Regex.IsMatch(input, pattern);
        }
    }
}
