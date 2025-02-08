using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Implementations
{
    public class SubModuleGetSingleHandler : ISubModuleGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ISubModuleQueryService _subModuleQueryService;
        public SubModuleGetSingleHandler(
            IMapper mapper,
            ISubModuleQueryService subModuleQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _subModuleQueryService = subModuleQueryService;
            _subModuleQueryService.NotNull(nameof(subModuleQueryService));
        }

        public async Task<SubModuleGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            var subModule = await _subModuleQueryService.GetInclude(id);
            return _mapper.Map<SubModuleGetDto>(subModule);
        }
    }


}