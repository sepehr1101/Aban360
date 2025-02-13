using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    internal sealed class LocationValueKeyQueryHandler : ILocationValueKeyQueryHandler
    {
        private readonly IZoneQueryService _zoneQueryService;
        public LocationValueKeyQueryHandler(IZoneQueryService zoneQueryService)
        {
            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));
        }
        public async Task<LocationValueKeyDto> Handle(CancellationToken cancellationToken)
        {
            ICollection<Zone> zones = await _zoneQueryService.GetIncludeAll();
            LocationValueKeyDto locationValueKeyDto = new (BuildLocationTree(zones));
            return locationValueKeyDto;
        }
        private List<CordinalDirectionValueKey> BuildLocationTree(ICollection<Zone> zones)
        {
            var directionDic = new Dictionary<int, CordinalDirectionValueKey>();
            var provinceDict = new Dictionary<int, ProvinceValueKey>();
            var headquartersDict = new Dictionary<int, HeadquartersValueKey>();
            var regionDict = new Dictionary<int, RegionValueKey>();

            // Build the tree from the bottom up (Zone -> Region -> Headquarters -> Province -> CordinalDirection)
            foreach (var zone in zones)
            {
                // Get or create the RegionTree
                if (!regionDict.TryGetValue(zone.RegionId, out var regionTree))
                {
                    regionTree = new RegionValueKey(zone.RegionId, zone.Region.Title, new List<ZoneValueKey>());                 
                    regionDict[zone.RegionId] = regionTree;
                }

                // Get or create the HeadquartersTree
                if (!headquartersDict.TryGetValue(zone.Region.HeadquartersId, out var headquartersTree))
                {
                    headquartersTree = new HeadquartersValueKey(zone.Region.Headquarters.Id,zone.Region.Headquarters.Title, new List<RegionValueKey>());                 
                    headquartersDict[zone.Region.HeadquartersId] = headquartersTree;
                }

                // Get or create the ProvinceTree
                if (!provinceDict.TryGetValue(zone.Region.Headquarters.ProvinceId, out var provinceTree))
                {
                    provinceTree = new ProvinceValueKey(zone.Region.Headquarters.Province.Id, zone.Region.Headquarters.Province.Title, new List<HeadquartersValueKey>());                  
                    provinceDict[zone.Region.Headquarters.ProvinceId] = provinceTree;
                }

                // Get or create the CountryTree
                if (!directionDic.TryGetValue(zone.Region.Headquarters.Province.CordinalDirectionId, out var directionTree))
                {
                    directionTree = new CordinalDirectionValueKey(zone.Region.Headquarters.Province.CordinalDirection.Id, zone.Region.Headquarters.Province.CordinalDirection.Title, new List<ProvinceValueKey>());                   
                    directionDic[zone.Region.Headquarters.Province.CordinalDirectionId] = directionTree;
                }

                // Add the zone to the region
                regionTree.ZoneValueKeys.Add(new ZoneValueKey(zone.Id, zone.Title));
             

                // Add the region to the headquarters (if not already added)
                if (!headquartersTree.RegionValueKeys.Contains(regionTree))
                {
                    headquartersTree.RegionValueKeys.Add(regionTree);
                }

                // Add the headquarters to the province (if not already added)
                if (!provinceTree.HeadquartersValueKeys.Contains(headquartersTree))
                {
                    provinceTree.HeadquartersValueKeys.Add(headquartersTree);
                }

                // Add the province to the country (if not already added)
                if (!directionTree.ProvinceValueKeys.Contains(provinceTree))
                {
                    directionTree.ProvinceValueKeys.Add(provinceTree);
                }
            }
            return directionDic.Values.ToList();
        }
    }
}
