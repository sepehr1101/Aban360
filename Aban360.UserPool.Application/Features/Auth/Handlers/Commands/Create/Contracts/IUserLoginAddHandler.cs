using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts
{
    public interface IUserLoginAddHandler
    {
        Task<FirstStepOutput> Handle(FirstStepLoginInput input, User user);
    }
}