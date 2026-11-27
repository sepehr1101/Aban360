namespace Aban360.MeterPool.Domain.Features.Management.Dtos.Queries
{
    public record MeterApkValidateOutputDto
    {
        public bool IsVersionValid { get; set; }
        public string ServerVersion { get; set; }
        public string UserVersion { get; set; }
        public MeterApkValidateOutputDto(bool isVersionValid, string serverVersion, string userVersion)
        {
            IsVersionValid = isVersionValid;
            ServerVersion = serverVersion;
            UserVersion = userVersion;
        }
        public MeterApkValidateOutputDto()
        {
        }
    }
}
