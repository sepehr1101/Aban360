using Aban360.Common.Exceptions;
using Aban360.Common.Literals;

namespace Aban360.Api.Cronjobs
{
    internal static class FileRemover 
    {   
        public static void DeleteOldFiles(int tresholdDay, string filePath, string searchPattern)
        {           

            if (!Directory.Exists(filePath))
            {
                throw new BaseException(ExceptionLiterals.NotFoundAddress);
            }

            var files = Directory.GetFiles(filePath, searchPattern, SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                DateTime creationTime = File.GetCreationTime(file);
                if (creationTime < DateTime.Now.AddDays(-tresholdDay))
                {
                    File.Delete(file);//todo: Check Permission
                }
            }
        }
    }
}
