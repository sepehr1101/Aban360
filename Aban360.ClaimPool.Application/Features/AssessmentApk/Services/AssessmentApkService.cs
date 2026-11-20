using Aban360.ClaimPool.Domain.Features.AssessmentApk.Queries;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;

namespace Aban360.ClaimPool.Application.Features.AssessmentApk.Services;
public interface IAssessmentApkService
{
    (FileStream, string) AssessmentApkLatestDownload(CancellationToken cancellationToken);
    AssessmentApkValidateOutputDto AssessmentApkValidateVersion(string userVersion, CancellationToken cancellationToken);
}

internal sealed class AssessmentApkService : IAssessmentApkService
{
    private string _baseFolderPath = DirectoryLiterals.AssessmentApkLogsPath;
    private string _assessmentApkValidateVersionbaseFolderPath = @"AppData\AssessmentApk";
    public AssessmentApkService()
    {
    }

    public (FileStream, string) AssessmentApkLatestDownload(CancellationToken cancellationToken)
    {
        string[] versions = Directory.GetDirectories(_baseFolderPath);
        string latestVersion = Path.GetFileName(versions[0]);
        string apkPath = Path.Combine(_baseFolderPath, latestVersion);
        string[] apks = Directory.GetFiles(apkPath);
        string applicationName = Path.GetFileName(apks[0]);
        string filePath = Path.Combine(_baseFolderPath, latestVersion, applicationName);
        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return (fileStream, applicationName);
    }

    public AssessmentApkValidateOutputDto AssessmentApkValidateVersion(string userVersion, CancellationToken cancellationToken)
    {
        //string userFilePath = Path.Combine(_baseFolderPath, userVersion);
        //if (!Directory.Exists(userFilePath))
        //{
        //    throw new AssessmentException(ExceptionLiterals.NotFoundFolder);
        //}
        string serverFilePath = Path.Combine(_assessmentApkValidateVersionbaseFolderPath);
        if (!Directory.Exists(serverFilePath))
        {
            throw new AssessmentException(ExceptionLiterals.NotFoundFolder);
        }

        string[] versions = Directory.GetDirectories(serverFilePath);
        string latestVersion = Path.GetFileName(versions[0]);
        bool isValid = latestVersion == userVersion;
        return new AssessmentApkValidateOutputDto(isValid, latestVersion, userVersion);
    }
}
