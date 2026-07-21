using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts
{
    public interface IMeterReadingNonReadCreateHandler
    {
        Task<MeterReadingNonReadOutputDto> Handle(MeterReadingNonReadInputDto input, IAppUser appUser, CancellationToken cancellationToken);
    }
}
