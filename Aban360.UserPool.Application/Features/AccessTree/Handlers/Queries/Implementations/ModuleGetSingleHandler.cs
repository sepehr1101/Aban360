using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Implementations
{
    internal sealed class ModuleGetSingleHandler : IModuleGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IModuleQueryService _moduleQueryService;
        public ModuleGetSingleHandler(
            IMapper mapper,
            IModuleQueryService moduleQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _moduleQueryService = moduleQueryService;
            _moduleQueryService.NotNull(nameof(moduleQueryService));
        }

        public async Task<ModuleGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            Module module = await _moduleQueryService.GetInclude(id);
            return _mapper.Map<ModuleGetDto>(module);
        }
    }
}
