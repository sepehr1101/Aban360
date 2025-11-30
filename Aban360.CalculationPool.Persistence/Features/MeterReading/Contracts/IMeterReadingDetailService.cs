using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts
{
    public interface IMeterReadingDetailService
    {
        Task Insert(IEnumerable<MeterReadingDetailCreateDto> input);
        Task Update(IEnumerable<MeterReadingWithAbBahaResultUpdateDto> input);
        Task Delete(MeterReadingDetailDeleteDto input);
        Task CreateDuplicateForLog(MeterReadingDetailCreateDuplicateDto input);
        Task Exclude(MeterReadingDetailExcludedDto input);
        Task<IEnumerable<MeterReadingDetailDataOutputDto>> Get(int flowImportedId);
    }
}
