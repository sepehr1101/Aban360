using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Commands.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Implementations
{
    internal sealed class SubModuleCreateHandler : ISubModuleCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISubModuleCommandService _subModuleCommandService;
        public SubModuleCreateHandler(
            IMapper mapper,
            ISubModuleCommandService subModuleCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _subModuleCommandService = subModuleCommandService;
            _subModuleCommandService.NotNull(nameof(subModuleCommandService));
        }

        public async Task Handle(SubModuleCreateDto createDto, CancellationToken cancellationToken)
        {
            SubModule subModule = _mapper.Map<SubModule>(createDto);
            subModule.IsActive = true;
            await _subModuleCommandService.Add(subModule);
        }
    }
}