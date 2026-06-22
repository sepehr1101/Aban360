using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts
{
    public interface IMeterReadingDetailQueryService
    {
        Task<IEnumerable<MeterReadingDetailDataOutputDto>> GetWithoutExcluded(int flowImportedId);
        Task<MeterReadingDetailDataOutputDto> GetById(int id);
        Task<IEnumerable<MeterReadingDetailExcludedDataOutptuDto>> Get(MeterReadingDetailExcludedInputDto inputDto);
    }
}
