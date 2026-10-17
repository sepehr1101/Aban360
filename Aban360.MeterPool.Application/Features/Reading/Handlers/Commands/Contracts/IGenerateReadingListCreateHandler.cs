using Aban360.Common.ApplicationUser;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Contracts
{
    public interface IGenerateReadingListCreateHandler
    {
        Task<GenerateReadingListOutputDto> Handle(GenerateReadingListInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}