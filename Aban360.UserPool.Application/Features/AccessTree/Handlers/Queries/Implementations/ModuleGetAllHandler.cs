using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Implementations
{
    internal sealed class ModuleGetAllHandler : IModuleGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IModuleQueryService _moduleQueryService;
        public ModuleGetAllHandler(
            IMapper mapper,
            IModuleQueryService moduleQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _moduleQueryService = moduleQueryService;
            _moduleQueryService.NotNull(nameof(moduleQueryService));
        }

        public async Task<ICollection<ModuleGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<Module> module = await _moduleQueryService.GetInclude();
            return _mapper.Map<ICollection<ModuleGetDto>>(module);
        }
    }
}
