using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using DNTPersianUtils.Core;
using System.Text;

namespace Aban360.SystemPool.Application.Features.Logging.Services;

public interface IAssessmentLogService
{
    Task SaveAsync(AssessmentLogInsertDto input, CancellationToken cancellationToken);
    IEnumerable<AssessmentLogGetDto> GetAll(CancellationToken cancellationToken);
    AssessmentLogFileGetDto GetByFileName(string fileName, CancellationToken cancellationToken);
}

internal sealed class AssessmentLogService : IAssessmentLogService
{
    private string _path = @"AppData\AssessmentLogs";
    private string _folderPath = @"AppData\AssessmentLogs";
    private string _mimeType = @"*.txt";
    public AssessmentLogService()
    {
    }

    public async Task SaveAsync(AssessmentLogInsertDto input, CancellationToken cancellationToken)
    {
        string path = await IoExtensions.SaveToDisk(input.File, _path);
    }

    public IEnumerable<AssessmentLogGetDto> GetAll(CancellationToken cancellationToken)
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

    public AssessmentLogFileGetDto GetByFileName(string fileName, CancellationToken cancellationToken)
    {
        string fullPath = Path.Combine(_folderPath, fileName);
        if (!File.Exists(fullPath))
        {
            throw new InvalidTrackingException(ExceptionLiterals.NotFoundFile);
        }

        FileInfo fileInfo = new FileInfo(fullPath);
        string content = File.ReadAllText(fullPath, Encoding.UTF8);
        return new AssessmentLogFileGetDto(fileName, content, fileInfo.LastWriteTime.ToPersianDateTimeString("yyyy/MM/dd - hh:mm:ss"));
    }
}
