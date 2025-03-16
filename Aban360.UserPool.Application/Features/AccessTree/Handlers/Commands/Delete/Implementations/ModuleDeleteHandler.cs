using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Implementations
{
    internal sealed class ModuleDeleteHandler : IModuleDeleteHandler
    {
        private readonly IModuleQueryService _moduleQueryService;
        private readonly IModuleCommandService _moduleCommandService;
        public ModuleDeleteHandler(
            IModuleQueryService moduleQueryService,
            IModuleCommandService moduleCommandService)
        {
            _moduleQueryService = moduleQueryService;
            _moduleQueryService.NotNull(nameof(moduleQueryService));

            _moduleCommandService = moduleCommandService;
            _moduleCommandService.NotNull(nameof(moduleCommandService));
        }

        public async Task Handle(ModuleDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            Module module = await _moduleQueryService.Get(deleteDto.Id);
            _moduleCommandService.Remove(module);
        }
    }
}
