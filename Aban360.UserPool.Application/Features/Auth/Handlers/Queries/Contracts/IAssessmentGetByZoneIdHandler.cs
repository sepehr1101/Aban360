using Aban360.Common.BaseEntities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface IAssessmentGetByZoneIdHandler
    {
        Task<IEnumerable<StringDictionary>> Handle(int zoneId, CancellationToken cancellationToken);
    }
}
