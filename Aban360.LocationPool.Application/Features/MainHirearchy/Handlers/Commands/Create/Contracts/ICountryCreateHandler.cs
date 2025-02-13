using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Create.Contracts
{
    public interface ICountryCreateHandler
    {
        Task Handle(CountryCreateDto countryCreateDto, CancellationToken cancellationToken);
    }
}
