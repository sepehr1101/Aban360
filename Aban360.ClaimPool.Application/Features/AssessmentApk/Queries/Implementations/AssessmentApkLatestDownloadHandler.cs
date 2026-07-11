using Aban360.ClaimPool.Application.Features.AssessmentApk.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;

namespace Aban360.ClaimPool.Application.Features.AssessmentApk.Queries.Implementations
{
    internal sealed class AssessmentApkLatestDownloadHandler : IAssessmentApkLatestDownloadHandler
    {
        private string _baseFolderPath = @"AppData\AssessmentApk";
        public (FileStream, string) Handle(CancellationToken cancellationToken)
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
    }
}