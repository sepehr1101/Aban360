using Aban360.Common.Exceptions;
using Aban360.Common.Literals;

namespace Aban360.Common.Db.Services
{
    public static class UserService
    {
        public static int GetUserCode(string userName)
        {
            bool isSuccess = int.TryParse(userName, out int userCode);
            if (!isSuccess)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidUserName);
            }

            return userCode;
        }
    }
}
