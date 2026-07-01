using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts
{
    public interface IMeterReadingCreateBaseHandler
    {
        Task<ICollection<MeterReadingDetailCreateDto>> GetReadingDetailCreateFinal(IEnumerable<MeterReadingDetailCreateDto> readingDetails, FileCreateDto fileInfo, IAppUser appUser, CancellationToken cancellationToken);
        Task<ICollection<MeterReadingDetailCreateDto>> GetReadingDetailCreateFinalNonRead(IEnumerable<MeterReadingDetailCreateDto> readingDetails, FileCreateDto fileInfo, IAppUser appUser, CancellationToken cancellationToken);
        Task<(CustomersInfoGetDto, int)> GetCustomerInfoAndFirstFlowId(ICollection<MeterReadingFileDetail> meterReadings, string fileName, string filePath, string? description, Guid userId);
        IEnumerable<MeterReadingDetailCreateDto> GetReadingMeterDetails(ICollection<MeterReadingFileDetail> meterReadings, CustomersInfoGetDto customersInfo, int meterFlowId);
        Task ExecSql(ICollection<MeterReadingDetailCreateDto> readingDetailsCreate, FileCreateDto fileInfo, IAppUser appUser);
        ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> GetReturnData(IEnumerable<MeterReadingDetailCreateDto> data, string title);
        Task CheckDuplicateFile(string fileName, CancellationToken cancellationToken);
        MeterFlowCreateDto GetMeterFlowCreateDto(MeterFlowStepEnum step, string fileName, int zoneId, string fromReadingNumber, string toReadingNumber, Guid userId, string description);
        MeterReadingFileDetail CreateMeterReading(int zoneId, int customerNumber, string readingNumber, int agentCode, short currentCounterStateCode, string previousDateJalali, string currentDateJalali, int previousNumber, int currentNumber, Guid userId);

    }
}