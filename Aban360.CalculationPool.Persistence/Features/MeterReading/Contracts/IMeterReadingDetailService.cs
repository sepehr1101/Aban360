using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts
{
    public interface IMeterReadingDetailService
    {
        Task Create(IEnumerable<MeterReadingDetailCreateDto> input);
        Task Update(IEnumerable<MeterReadingWithAbBahaResultUpdateDto> input);
        Task Delete(MeterReadingDetailDeleteDto input);
        Task CreateDuplicateForLog(MeterReadingDetailCreateDuplicateDto input);
        Task UpdateToExcluded(MeterReadingDetailExcludedDto input);
        Task<IEnumerable<MeterReadingDetailGetDto>> Get(int flowImportedId);
    }
}
