using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class ReadingBlockListGetHandler : IReadingBlockListGetHandler
    {
        public IEnumerable<StringDictionary> Handle(CancellationToken cancellationToken)
        {
            return [
            new StringDictionary("ندارد","ندارد"),
            new StringDictionary("A", "A"),
            new StringDictionary("B", "B"),
            new StringDictionary("C", "C"),
            new StringDictionary("D", "D"),
            new StringDictionary("E", "E"),
            new StringDictionary("F", "F"),
            new StringDictionary("G", "G"),
            new StringDictionary("H", "H"),
            new StringDictionary("I", "I"),
            new StringDictionary("J", "J"),
            new StringDictionary("K", "K"),
            new StringDictionary("L", "L"),
            new StringDictionary("M", "M"),
            new StringDictionary("N", "N")];
        }
    }
}
