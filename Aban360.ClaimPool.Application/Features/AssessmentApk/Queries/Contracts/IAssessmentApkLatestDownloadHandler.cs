namespace Aban360.ClaimPool.Application.Features.AssessmentApk.Queries.Contracts
{
    public interface IAssessmentApkLatestDownloadHandler
    {
        (FileStream, string) Handle(CancellationToken cancellationToken);
    }
}