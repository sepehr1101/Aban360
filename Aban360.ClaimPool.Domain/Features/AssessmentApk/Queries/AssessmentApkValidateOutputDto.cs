namespace Aban360.ClaimPool.Domain.Features.AssessmentApk.Queries
{
    public record AssessmentApkValidateOutputDto
    {
        public bool IsVersionValid { get; set; }
        public string ServerVersion { get; set; }
        public string UserVersion { get; set; }
        public AssessmentApkValidateOutputDto(bool isVersionValid, string serverVersion, string userVersion)
        {
            IsVersionValid = isVersionValid;
            ServerVersion = serverVersion;
            UserVersion = userVersion;
        }
        public AssessmentApkValidateOutputDto()
        {
        }
    }
}
