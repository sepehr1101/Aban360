using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;

namespace Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Conracts
{
    public interface IAssessmentLogGetByFileNameHandler
    {
        AssessmentLogFileGetDto Handle(string fileName, CancellationToken cancellationToken);
    }
}
