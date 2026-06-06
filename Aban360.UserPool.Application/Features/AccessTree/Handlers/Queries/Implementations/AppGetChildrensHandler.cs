using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Implementations
{
    internal sealed class AppGetChildrensHandler : IAppGetChildrensHandler
    {
        private readonly IMapper _mapper;
        private readonly IAppQueryService _appQueryService;
        public AppGetChildrensHandler(
            IMapper mapper,
            IAppQueryService appQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _appQueryService = appQueryService;
            _appQueryService.NotNull(nameof(appQueryService));
        }

        public async Task<IEnumerable<AppWithModuleGetDto>> Handle(int id, CancellationToken cancellationToken)
        {
            IEnumerable<App> app = await _appQueryService.GetChildrens(id);
            return _mapper.Map<ICollection<AppWithModuleGetDto>>(app);
        }
    }
}
