using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.CollectBills.Outputs;

namespace Aban360.CalculationPool.Infrastructure.Providers.CollectBills.Contracts
{
    public interface ICollectBillsService
    {
        Task<CollectBillsOutputDto<object>> SendCustomerInfo(CollectBillsSubscriptionInfoSendInputDto sampleInputDto);
        Task<CollectBillsOutputDto<CollectBillsUploadOutputDto>> Upload(CollectBillsUploadInputDto input);
        Task<CollectBillsOutputDto<CollectBillsAssignUploadedFileOutputDto>> AssignUploadedFile(CollectBillsAssignUploadedFileInputDto input);
        Task<CollectBillsOutputDto<CollectBillsConfirmFileOutputDto>> ConfirmFileBills(CollectBillsConfirmFileInputDto input);
        Task<CollectBillsOutputDto<CollectBillsServerConfigOutputDto>> GetLastSubscriptionInfoByBillId(string billId);
        Task<CollectBillsOutputDto<CollectBillsFileDetailOutputDto>> GetFileDetails(CollectBillsFileDetailInputDto input);
    }
}
