using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface IAssessmentGetByZoneIdHandler
    {
        Task<IEnumerable<StringDictionary>> Handle(int zoneId, CancellationToken cancellationToken);
        Task<IEnumerable<StringDictionary>> Handle(IAppUser appUser, CancellationToken cancellationToken);
    }
}
