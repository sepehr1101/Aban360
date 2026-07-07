using Aban360.Common.BaseEntities;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts
{
    public interface IBedBesQueryService
    {
        Task<BedBesConsumptionOutputDto> Get(string billId);
        Task<BedBesDataInfoOutptuDto> GetInAtlas(int id);
        Task<BedBesWithAmountOutputDto> GetLatest(ZoneIdAndCustomerNumberOutputDto input);
        Task<IEnumerable<BillsCanRemoveOutputDto>> GetToRemove(RemovedBillSearchDto input);
        Task<RemoveBillDataInputDto> GetToRemove(RemoveBillGetDto input);
        Task<IEnumerable<BillsCanReturnOutputDto>> GetToReturned(ReturnBillSearchDto input);
        Task<IEnumerable<BillsCanRemoveOutputDto>> Get(BillToReturnInputDto input);
        Task<ReportOutput<ManualBillHeaderOutputDto, ManualBillDataOutputDto>> Get(ManualBillInputDto input);
        Task<float> GetPreviousBill(int zoneId, int customerNumber, string dateJalali);
        Task<float?> GetAverage(int zoneId, int customerNumber, string fromDate, string toDate);
        Task<IEnumerable<BedBesCreateDto>> Get(ZoneCustomerFromToDateDto input);
        Task<int> GetCountInDateBed(int zoneId, int customernumber, string date, bool isPreviousDate);
        Task<int?> GetLatestJalaseNumber(ZoneIdAndCustomerNumberOutputDto input);
        Task<BedBesSmsDto> GetSmsDto(string billId, int zoneId, int customerNumber);
        Task<BedBesWithConsumptionOutputDto> GetPrevious(ZoneIdAndCustomerNumberOutputDto input, string dateJalali);
        Task<IEnumerable<PreviousBillsInfoDto>> GetPreviousBillsInfo(ZoneIdAndCustomerNumber input);
        Task<IEnumerable<string>> GetDuplicateBill(ICollection<BedBesCreateDto> inputDto);
        Task<BedBesItemsOutputDto> GetLatestByCustomerNumber(ZoneIdAndCustomerNumber input);
        Task<IEnumerable<BedBesWithDelOutputDto>> GetByDateInterval(ZoneCustomerFromToDateDto input, string dbName);
        Task<BedBesPreviousNumberAndDateOutputDto?> GetPreviousDateAndNumber(ZoneIdAndCustomerNumber input, string billId, bool hasException);
        Task<IEnumerable<ZoneIdAndCustomerNumber>> GetPreviousDateAndNumberWithSqlBulk(IDbConnection connection, IDbTransaction transaction, int zoneId, ICollection<int> customerNumbers);
    }
}
