using Aban360.Common.ApplicationUser;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Contracts
{
    public interface IReaderCartableGetHandler
    {
        Task<ReaderCartableOutputDto> Handle(IAppUser appUser, CancellationToken cancellationToken);
    }
}
