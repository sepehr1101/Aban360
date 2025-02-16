using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts
{
    public interface ICaptchaUpdateHandler
    {
        Task Handle(CaptchaUpdateDto capthcaUpdateDto, CancellationToken cancellationToken);
    }
}