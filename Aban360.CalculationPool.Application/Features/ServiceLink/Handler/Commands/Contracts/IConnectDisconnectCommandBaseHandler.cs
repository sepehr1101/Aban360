using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    public interface IConnectDisconnectCommandBaseHandler
    {
        Task<ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto>> ValidateAndGetCustomerGeneral(string billId, int? why, bool isConnect);
        Task<(ConCompanyGetDto, ConCompanyPersonnelPersonalGetDto)> GetCompanyInfo(int companyId, Guid personnelId);
        Task<ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto>> GetResult(ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo, string billId, int? why, string title, string messageText, string companyName, CancellationToken cancellationToken);
        ConnectDisconnectInsertDto GetConnectDisconnectInsertDto(ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo, int? why, string? description, ConCompanyGetDto companyInfo, ConCompanyPersonnelPersonalGetDto personnelInfo, IAppUser appUser, string causeTitle, int typeId, string typeTitle);
        Task ExecSql(IDbConnection connection, IDbTransaction transaction, ConnectDisconnectInsertDto connectDisconnectInsertDto, IAppUser appUser, string logTex);
        ICollection<NumericDictionary> GetCasues();
    }
}
