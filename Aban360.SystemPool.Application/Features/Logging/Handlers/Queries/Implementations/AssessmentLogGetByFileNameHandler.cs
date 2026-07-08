using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Conracts;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using DNTPersianUtils.Core;
using System.Text;

namespace Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Implementations
{
    internal sealed class AssessmentLogGetByFileNameHandler : IAssessmentLogGetByFileNameHandler
    {
        private string _folderPath = @"AppData\AssessmentLogs";
        public AssessmentLogFileGetDto Handle(string fileName,CancellationToken cancellationToken)
        {
            string fullPath = Path.Combine(_folderPath, fileName);
            if (!File.Exists(fullPath))
            {
                throw new InvalidTrackingException(ExceptionLiterals.NotFoundFile);
            }
            FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            


            FileInfo fileInfo = new FileInfo(fullPath);
            string content = File.ReadAllText(fullPath, Encoding.UTF8);
            return new AssessmentLogFileGetDto(fileName,content,fileInfo.LastWriteTime.ToPersianDateTimeString("yyyy/MM/dd - hh:mm:ss"));
        }
    }
}
