using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Commands.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Implementations
{
    internal sealed class ModuleCreateHandler : IModuleCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IModuleCommandService _moduleCommandService;
        public ModuleCreateHandler(
            IMapper mapper,
            IModuleCommandService moduleCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _moduleCommandService = moduleCommandService;
            _moduleCommandService.NotNull(nameof(moduleCommandService));
        }

        public async Task Handle(ModuleCreateDto createDto, CancellationToken cancellationToken)
        {
            Module module = _mapper.Map<Module>(createDto);
            await _moduleCommandService.Add(module);
        }
    }
}
