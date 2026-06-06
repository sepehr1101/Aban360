using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Modulelication.Features.AccessTree.Handlers.Queries.Implementations
{
    internal sealed class ModuleGetChildrensHandler : IModuleGetChildrensHandler
    {
        private readonly IMapper _mapper;
        private readonly IModuleQueryService _moduleQueryService;
        public ModuleGetChildrensHandler(
            IMapper mapper,
            IModuleQueryService moduleQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _moduleQueryService = moduleQueryService;
            _moduleQueryService.NotNull(nameof(moduleQueryService));
        }

        public async Task<IEnumerable<ModuleWithSubModuleGetDto>> Handle(int id, CancellationToken cancellationToken)
        {
            IEnumerable<Module> Module = await _moduleQueryService.GetChildrens(id);
            return _mapper.Map<ICollection<ModuleWithSubModuleGetDto>>(Module);
        }
    }
}
