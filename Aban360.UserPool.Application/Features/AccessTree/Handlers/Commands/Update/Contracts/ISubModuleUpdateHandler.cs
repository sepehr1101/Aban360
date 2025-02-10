﻿using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Update.Contracts
{
    public interface ISubModuleUpdateHandler
    {
        Task Handle(SubModuleUpdateDto updateDto, CancellationToken cancellationToken);
    }


}