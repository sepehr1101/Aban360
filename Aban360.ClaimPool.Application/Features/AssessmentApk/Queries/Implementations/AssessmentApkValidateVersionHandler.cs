using Aban360.ClaimPool.Application.Features.AssessmentApk.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.AssessmentApk.Queries;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;

namespace Aban360.ClaimPool.Application.Features.AssessmentApk.Queries.Implementations
{
    internal sealed class AssessmentApkValidateVersionHandler : IAssessmentApkValidateVersionHandler
    {
        private string _baseFolderPath = @"AppData\AssessmentApk";
        public AssessmentApkValidateOutputDto Handle(string userVersion, CancellationToken cancellationToken)
        {
            //string userFilePath = Path.Combine(_baseFolderPath, userVersion);
            //if (!Directory.Exists(userFilePath))
            //{
            //    throw new AssessmentException(ExceptionLiterals.NotFoundFolder);
            //}
            string serverFilePath = Path.Combine(_baseFolderPath);
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
}
