using Aban360.Common.Exceptions;
using Aban360.Common.Literals;

namespace Aban360.Api.Cronjobs
{
    public interface IFileRemover
    {
        void DeleteOldExcelFiles();
    }

    internal sealed class FileRemover : IFileRemover
    {
        private readonly IConfiguration _configuration;
        public FileRemover(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void DeleteOldExcelFiles()
        {
            var tresholdDay = int.Parse(_configuration["FileManagement:ExcelExpireDay"]);
            var excelFilePath = _configuration["FileManagement:ExcelPath"].ToString();

            if (!Directory.Exists(excelFilePath))
            {
                throw new BaseException(ExceptionLiterals.NotFoundAddress);
            }

            var files = Directory.GetFiles(excelFilePath, "*.xlsx", SearchOption.TopDirectoryOnly);

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
