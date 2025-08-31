using Aban360.Api.Exceptions;
using Aban360.Common.Literals;

namespace Aban360.Api.Cronjobs
{
    internal static class FileRemover 
    {   
        public static void DeleteOldFiles(int tresholdDay, string filePath, string searchPattern)
        {           
            if (!Directory.Exists(filePath))
            {
                throw new InvalidFilePathException(ExceptionLiterals.NotFoundAddress);
            }

            var files = Directory.GetFiles(filePath, searchPattern, SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                DateTime creationTime = File.GetCreationTime(file);
                if (creationTime < DateTime.Now.AddDays(-tresholdDay))
                {
                    File.Delete(file);//TODO: Check Permission
                }
            }
        }
    }
}
