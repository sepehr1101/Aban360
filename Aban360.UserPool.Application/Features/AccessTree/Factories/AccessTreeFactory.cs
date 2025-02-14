using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;

namespace Aban360.UserPool.Application.Features.AccessTree.Factories
{
    internal static class AccessTreeFactory
    {
        internal static AccessTreeValueKeyDto CreateAccessTree(this ICollection<Endpoint> endpoints)
        {
            var appValueKeys= endpoints.BuildAccessTree(new List<int>());
            return new AccessTreeValueKeyDto(appValueKeys);
        }
        internal static AccessTreeValueKeyDto CreateAccessTree(this ICollection<Endpoint> endpoints, ICollection<int> selectedEndpointIds)
        {
            var appValueKeys = endpoints.BuildAccessTree(selectedEndpointIds);
            return new AccessTreeValueKeyDto(appValueKeys);
        }
        private static ICollection<AppValueKey> BuildAccessTree(this ICollection<Endpoint> endpoints, ICollection<int> selectedEndpointIds)
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

                var isEndpointSelected = (selectedEndpointIds is not null) && selectedEndpointIds.Contains(endpoint.Id);
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
