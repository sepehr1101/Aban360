using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Features.UiElement.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Implementations
{
    public class SubModuleDeleteHandler : ISubModuleDeleteHandler
    {
        private readonly ISubModuleQueryService _subModuleQueryService;
        private readonly ISubModuleCommandService _subModuleCommandService;
        public SubModuleDeleteHandler(
            ISubModuleQueryService subModuleQueryService,
            ISubModuleCommandService subModuleCommandService)
        {
            _subModuleQueryService = subModuleQueryService;
            _subModuleQueryService.NotNull(nameof(subModuleQueryService));

            _subModuleCommandService = subModuleCommandService;
            _subModuleCommandService.NotNull(nameof(subModuleCommandService));
        }

        public async Task Handle(SubModuleDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var subModule = await _subModuleQueryService.Get(deleteDto.Id);
            if (subModule == null)
            {
                throw new InvalidDataException();
            }
            _subModuleCommandService.Remove(subModule);
        }
    }


}