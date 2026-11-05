using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class MapDictionaryGetHandler : IMapDictionaryGetHandler
    {
        public IEnumerable<StringDictionary> Handle(CancellationToken cancellationToken)
        {
            return [new ("ماهواره","https://maps.abfaisfahan.com/api/tile/googlesatellite/"),
            new ("ناهمواری","https://maps.abfaisfahan.com/api/tile/googleterrain/"),
            new ("مسیر‌ها","https://maps.abfaisfahan.com/api/tile/osm/"),
            new ("گوگل","https://maps.abfaisfahan.com/api/tile/googlestreet/"),
            new ("هیبرید","https://maps.abfaisfahan.com/api/tile/googlehybrid/")];
        }
    }
}
