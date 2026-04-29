namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Contracts
{
    public interface IAssessmentOffRemoveHandler
    {
        Task Handle(Guid id, int userCode, CancellationToken cancellationToken);
    }
}
