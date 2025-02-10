﻿using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Update.Implementations
{
    public class ModuleUpdateHandler : IModuleUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IModuleQueryService _moduleQueryService;
        public ModuleUpdateHandler(
            IMapper mapper,
            IModuleQueryService moduleQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _moduleQueryService = moduleQueryService;
            _moduleQueryService.NotNull(nameof(moduleQueryService));
        }

        public async Task Handle(ModuleUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var module = await _moduleQueryService.Get(updateDto.Id);
            if (module == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, module);
        }
    }
}
