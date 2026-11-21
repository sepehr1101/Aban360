using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using DNTPersianUtils.Core;
using System.Text;

namespace Aban360.SystemPool.Application.Features.Logging.Services;

public interface IMeterApkLogService
{
    Task SaveAsync(MeterApkLogInsertDto input, CancellationToken cancellationToken);
    IEnumerable<MeterApkLogGetDto> GetAll(CancellationToken cancellationToken);
    MeterApkLogFileGetDto GetByFileName(string fileName, CancellationToken cancellationToken);
}

internal sealed class MeterApkLogService : IMeterApkLogService
{
    private string _path = @"AppData\MeterApkLogs";
    private string _folderPath = @"AppData\MeterApkLogs";
    private string _mimeType = @"*.txt";
    public MeterApkLogService()
    {
    }

    public async Task SaveAsync(MeterApkLogInsertDto input, CancellationToken cancellationToken)
    {
        string path = await IoExtensions.SaveToDisk(input.File, _path);
    }

    public IEnumerable<MeterApkLogGetDto> GetAll(CancellationToken cancellationToken)
    {
        string filePath = Path.Combine(_folderPath);
        if (!Directory.Exists(filePath))
        {
            throw new InvalidTrackingException(ExceptionLiterals.NotFoundFolder);
        }

        ICollection<MeterApkLogGetDto> result = new List<MeterApkLogGetDto>();
        string[] txtFiles = Directory.GetFiles(filePath, _mimeType, SearchOption.TopDirectoryOnly);
        foreach (string file in txtFiles)
        {
            FileInfo fileInfo = new FileInfo(file);
            DateTime lastWrite = fileInfo.LastWriteTime;
            result.Add(new MeterApkLogGetDto(fileInfo.Name, lastWrite.ToPersianDateTimeString("yyyy/MM/dd - hh:mm:ss")));
        }

        return result;
    }

    public MeterApkLogFileGetDto GetByFileName(string fileName, CancellationToken cancellationToken)
    {
        string fullPath = Path.Combine(_folderPath, fileName);
        if (!File.Exists(fullPath))
        {
            throw new InvalidTrackingException(ExceptionLiterals.NotFoundFile);
        }

        FileInfo fileInfo = new FileInfo(fullPath);
        string content = File.ReadAllText(fullPath, Encoding.UTF8);
        return new MeterApkLogFileGetDto(fileName, content, fileInfo.LastWriteTime.ToPersianDateTimeString("yyyy/MM/dd - hh:mm:ss"));
    }
}
