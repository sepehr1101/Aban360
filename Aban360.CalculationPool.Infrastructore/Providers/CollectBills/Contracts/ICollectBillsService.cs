using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.CollectBills.Outputs;

namespace Aban360.CalculationPool.Infrastructure.Providers.CollectBills.Contracts
{
    public interface ICollectBillsService
    {
        Task<CollectBillsOutputDto<CollectBillsUploadOutputDto>> Upload(CollectBillsUploadInputDto input);
        Task<CollectBillsOutputDto<CollectBillsAssignUploadedFileOutputDto>> AssignUploadedFile(CollectBillsAssignUploadedFileInputDto input);
        Task<CollectBillsOutputDto<CollectBillsFileDetailOutputDto>> GetFileDetails(CollectBillsFileDetailInputDto input);
        Task<CollectBillsOutputDto<CollectBillsServerConfigOutputDto>> GetServiceConfigForPanel(CollectBillsIdentityInputDto input);
        Task<CollectBillsOutputDto<CollectBillsConfirmFileOutputDto>> ConfirmFileBills(CollectBillsConfirmFileInputDto input);
    }
}
