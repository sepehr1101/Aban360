using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Features.UiElement.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Implementations
{
    public class AppDeleteHandler : IAppDeleteHandler
    {
        private readonly IAppCommandService _appCommandService;
        private readonly IAppQueryService _appQueryService;
        public AppDeleteHandler(
            IAppCommandService appCommandService,
            IAppQueryService appQueryService)
        {
            _appCommandService = appCommandService;
            _appCommandService.NotNull(nameof(appCommandService));

            _appQueryService = appQueryService;
            _appQueryService.NotNull(nameof(appQueryService));
        }

        public async Task Handle(AppDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var app = await _appQueryService.Get(deleteDto.Id);
            if (app == null)
            {
                throw new InvalidDataException();
            }
            _appCommandService.Remove(app);
        }
    }
}
