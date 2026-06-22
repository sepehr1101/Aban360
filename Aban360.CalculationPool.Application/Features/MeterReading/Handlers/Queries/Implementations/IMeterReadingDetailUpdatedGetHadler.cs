using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    public interface IMeterReadingDetailUpdatedGetHadler
    {
        Task<ReportOutput<MeterReadingDetailUpdatedHeaderOutptuDto, MeterReadingDetailUpdatedDataOutputDto>> Handle(MeterReadingDetailUpdatedInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}