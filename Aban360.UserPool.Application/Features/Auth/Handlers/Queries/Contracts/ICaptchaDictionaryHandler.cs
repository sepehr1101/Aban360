using Aban360.Common.BaseEntities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface ICaptchaDictionaryHandler
    {
        Task<ICollection<NumericDictionary>> Handle(CancellationToken cancellationToken);
    }
}