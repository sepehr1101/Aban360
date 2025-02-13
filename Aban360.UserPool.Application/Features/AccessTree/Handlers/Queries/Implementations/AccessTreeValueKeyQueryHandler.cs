using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Implementations
{
    internal sealed class AccessTreeValueKeyQueryHandler : IAccessTreeValueKeyQueryHandler
    {
        private readonly IEndpointQueryService _endpointQueryService;
        public AccessTreeValueKeyQueryHandler(IEndpointQueryService endpointQueryService)
        {
            _endpointQueryService = endpointQueryService;
            _endpointQueryService.NotNull(nameof(endpointQueryService));
        }
        public async Task<AccessTreeValueKeyDto> Handle(CancellationToken cancellationToken)
        {
            var endpoints = await _endpointQueryService.GetIncludeAll();
            var appValueKeys = BuildAccessTree(endpoints, new List<int>());
            return new AccessTreeValueKeyDto(appValueKeys);
        }
        public async Task<AccessTreeValueKeyDto> Handle(ICollection<int> selectedEndpointIds, CancellationToken cancellationToken)
        {
            var endpoints = await _endpointQueryService.GetIncludeAll();
            var appValueKeys = BuildAccessTree(endpoints, selectedEndpointIds);
            return new AccessTreeValueKeyDto(appValueKeys);
        }
        private ICollection<AppValueKey> BuildAccessTree(ICollection<Endpoint> endpoints, ICollection<int> selectedEndpointIds)
        {
            var appDict = new Dictionary<int, AppValueKey>();
            var moduleDict = new Dictionary<int, ModuleValueKey>();
            var subModuleDict = new Dictionary<int, SubModuleValueKey>();

            foreach (var endpoint in endpoints)
            {
                if (!subModuleDict.TryGetValue(endpoint.SubModuleId, out var subModuleTree))
                {
                    subModuleTree = new SubModuleValueKey(endpoint.Id, endpoint.Title, endpoint.Style, new List<EndpointValueKey>());
                    subModuleDict[endpoint.SubModuleId] = subModuleTree;
                }
                if (!moduleDict.TryGetValue(endpoint.SubModule.ModuleId, out var moduleTree))
                {
                    moduleTree = new ModuleValueKey(endpoint.SubModule.Module.Id, endpoint.SubModule.Module.Title, endpoint.SubModule.Module.Style, new List<SubModuleValueKey>());
                    moduleDict[endpoint.SubModule.ModuleId] = moduleTree;
                }
                if (!appDict.TryGetValue(endpoint.SubModule.Module.AppId, out var appTree))
                {
                    appTree = new AppValueKey(endpoint.SubModule.Module.App.Id, endpoint.SubModule.Module.App.Title, endpoint.SubModule.Module.App.Style, new List<ModuleValueKey>());
                    appDict[endpoint.SubModule.Module.AppId] = appTree;
                }

                var isEndpointSelected=(selectedEndpointIds is not null) && selectedEndpointIds.Contains(endpoint.Id);
                subModuleTree.EndpointValueKeys.Add(new EndpointValueKey(endpoint.Id, endpoint.Title, endpoint.Style, isEndpointSelected));

                if (!moduleTree.SubModuleValueKeys.Contains(subModuleTree))
                {
                    moduleTree.SubModuleValueKeys.Add(subModuleTree);
                }
                if (!appTree.ModuleValueKeys.Contains(moduleTree))
                {
                    appTree.ModuleValueKeys.Add(moduleTree);
                }
            }
            return appDict.Values.ToList();
        }
    }
}
