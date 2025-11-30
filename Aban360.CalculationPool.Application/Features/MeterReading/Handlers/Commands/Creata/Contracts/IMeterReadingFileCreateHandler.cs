using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts
{
    public interface IMeterReadingFileCreateHandler
    {
        Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto>> Handle(MeterReadingFileCreateDto input, IAppUser appUser, CancellationToken cancellationToken);
    }
}
