using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.CollectBills.Outputs;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
namespace Aban360.CalculationPool.Application.Features.CollectBills.Services {
public interface ICollectBillsFilesService {
CollectBillsGetZipFileInfo CollectBillsDetailFileDownloadAsync(string fileName, CancellationToken cancellationToken);
Task<CollectBillsGetZipFileInfo> CollectBillsFileCreateAsync(string? reportDateJalali, IAppUser appUser, CancellationToken cancellationToken);
}}
namespace Aban360.CalculationPool.Application.Features.CollectBills.Handlers.Queries.Implementations {
public interface ICollectBillsDetailWithLastStepGetHadler {
Task<ReportOutput<CollectBillsDetailWithLastStepHeaderOutputDto, CollectBillsDetailWithLastStepDataOutputDto>> Handle(CollectBillsDetialReportInputDto inputDto, CancellationToken cancellationToken);
}}
