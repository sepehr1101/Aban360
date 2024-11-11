using Aban360.Common.Literals;

namespace Aban360.Common.Exceptions
{
    public class RootProjectNotFoundException:Exception       
    {
        public RootProjectNotFoundException(string applicationBasePath)
            :base($"{ExceptionLiterals.AppBasePathNotFound_1} {applicationBasePath}")
        {
            
        }
    }
}
