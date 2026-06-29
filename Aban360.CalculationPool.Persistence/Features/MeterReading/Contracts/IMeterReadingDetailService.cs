using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts
{
    public interface IMeterReadingDetailQueryService
    {
        Task<IEnumerable<MeterReadingDetailDataOutputDto>> Get(int flowImportedId, bool? hasExcluded);
        Task<MeterReadingDetailDataOutputDto> GetById(int id);
        Task<IEnumerable<MeterReadingDetailExcludedDataOutputDto>> Get(MeterReadingDetailExcludedInputDto inputDto);
        Task<IEnumerable<MeterReadingDetailUpdatedDataOutputDto>> GetUpdated(MeterReadingDetailUpdatedInputDto inputDto);
    }
}
