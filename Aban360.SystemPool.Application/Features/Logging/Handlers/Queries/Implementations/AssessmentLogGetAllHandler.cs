using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Conracts;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using DNTPersianUtils.Core;

namespace Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Implementations
{
    internal sealed class AssessmentLogGetAllHandler : IAssessmentLogGetAllHandler
    {
        private string _folderPath = @"AppData\AssessmentLogs";
        private string _mimeType = @"*.txt";
        public IEnumerable<AssessmentLogGetDto> Handle(CancellationToken cancellationToken)
        {
            string filePath = Path.Combine(_folderPath);
            if (!Directory.Exists(filePath))
            {
                throw new InvalidTrackingException(ExceptionLiterals.NotFoundFolder);
            }

            ICollection<AssessmentLogGetDto> result = new List<AssessmentLogGetDto>();
            string[] txtFiles = Directory.GetFiles(filePath, _mimeType, SearchOption.TopDirectoryOnly);
            foreach (string file in txtFiles)
            {
                FileInfo fileInfo = new FileInfo(file);
                DateTime lastWrite = fileInfo.LastWriteTime;

                result.Add(new AssessmentLogGetDto(fileInfo.Name, lastWrite.ToPersianDateTimeString("yyyy/MM/dd - hh:mm:ss")));
            }

            return result;
        }
    }
}
