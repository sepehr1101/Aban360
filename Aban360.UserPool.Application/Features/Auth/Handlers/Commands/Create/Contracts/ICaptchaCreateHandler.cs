using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts
{
    public interface ICaptchaCreateHandler
    {
        Task Handle(CaptchaCreateDto captchaCreateDto, CancellationToken cancellationToken);
    }
}
