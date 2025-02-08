using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Implementations
{
    public class AppGetSingleHandler : IAppGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IAppQueryService _appQueryService;
        public AppGetSingleHandler(
            IMapper mapper,
            IAppQueryService appQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _appQueryService = appQueryService;
            _appQueryService.NotNull(nameof(appQueryService));
        }

        public async Task<AppGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            var app = await _appQueryService.Get(id);
            return _mapper.Map<AppGetDto>(app);
        }
    }
}
