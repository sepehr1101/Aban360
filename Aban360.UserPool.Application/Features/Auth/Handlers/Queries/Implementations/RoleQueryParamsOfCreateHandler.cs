using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Factories;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    internal sealed class RoleQueryParamsOfCreateHandler : IRoleQueryParamsOfCreateHandler
    {
        private readonly IEndpointQueryService _endpointQueryService;
        public RoleQueryParamsOfCreateHandler(IEndpointQueryService endpointQueryService)
        {
            _endpointQueryService = endpointQueryService;
            _endpointQueryService.NotNull(nameof(endpointQueryService));
        }
        public async Task<RoleParamsOfCreateDto> Handle(CancellationToken cancellationToken)
        {
            AccessTreeValueKeyDto accessTree = await CreateAccessTree();
            RoleParamsOfCreateDto roleParamsOfCreateDto = new(accessTree);
            return roleParamsOfCreateDto;
        }
        private async Task<AccessTreeValueKeyDto> CreateAccessTree()
        {
            var endpoints = await _endpointQueryService.GetIncludeAll();
            var accessTree = endpoints.CreateAccessTree();
            return accessTree;
        }
    }
}
