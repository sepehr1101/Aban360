using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Commands.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Implementations
{
    internal sealed class AppCreateHandler : IAppCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IAppCommandService _appCommandService;
        public AppCreateHandler(
            IMapper mapper,
            IAppCommandService appCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _appCommandService = appCommandService;
            _appCommandService.NotNull(nameof(appCommandService));
        }

        public async Task Handle(AppCreateDto createDto, CancellationToken cancellationToken)
        {
            App app = _mapper.Map<App>(createDto);
            await _appCommandService.Add(app);
        }
    }
}
