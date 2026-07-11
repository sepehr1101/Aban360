using Aban360.ClaimPool.Domain.Features.AssessmentApk.Queries;

namespace Aban360.ClaimPool.Application.Features.AssessmentApk.Queries.Contracts
{
    public interface IAssessmentApkValidateVersionHandler
    {
        AssessmentApkValidateOutputDto Handle(string userVersion, CancellationToken cancellationToken);
    }
}
