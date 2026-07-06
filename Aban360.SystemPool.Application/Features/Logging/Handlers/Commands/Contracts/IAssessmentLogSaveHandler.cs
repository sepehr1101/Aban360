using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;

namespace Aban360.SystemPool.Application.Features.Logging.Handlers.Commands.Contracts
{
    public interface IAssessmentLogSaveHandler
    {
        Task Handle(AssessmentLogInsertDto input, CancellationToken cancellationToken);
    }
}
