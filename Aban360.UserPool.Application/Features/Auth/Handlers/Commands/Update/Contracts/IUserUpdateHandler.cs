using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts
{
    public interface IUserUpdateHandler
    {
        Task<UserPersonalGetDto> Handle(UserUpdateDto userUpdateDto, CancellationToken cancellationToken);
    }
}