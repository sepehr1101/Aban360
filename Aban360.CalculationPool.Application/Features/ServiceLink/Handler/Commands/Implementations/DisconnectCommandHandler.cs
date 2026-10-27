using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class DisconnectCommandHandler : AbstractBaseConnection, IDisconnectCommandHandler
    {
        private readonly IConnectDisconnectCommandBaseHandler _connectDisconnectCommandHandler;
        private int typeId = ReportLiterals.DisconnectId;
        private string typeTitle = ReportLiterals.Disconnect;
        public DisconnectCommandHandler(
            IConnectDisconnectCommandBaseHandler connectDisconnectCommandHandler,
            IConfiguration configuration)
                : base(configuration)
        {
            _connectDisconnectCommandHandler = connectDisconnectCommandHandler;
            _connectDisconnectCommandHandler.NotNull(nameof(connectDisconnectCommandHandler));
        }

        public async Task<ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto>> Handle(DisconnectPrintInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo = await _connectDisconnectCommandHandler.ValidateAndGetCustomerGeneral(inputDto.BillId, inputDto.Why, false);
            var (companyInfo, personnelInfo) = await _connectDisconnectCommandHandler.GetCompanyInfo(inputDto.CompanyId, inputDto.PersonnelId);
            NumericDictionary? connectDisconnectCause = _connectDisconnectCommandHandler.GetCasues().Where(c => c.Id == (inputDto.Why ?? 0)).FirstOrDefault();

            string messageText = string.Format(SmsTemplates.ServiceLinkDisconnectAlert, customerInfo.ReportData.FirstOrDefault().ZoneTitle, customerInfo.ReportHeader.FullName, customerInfo.ReportHeader.BillId, connectDisconnectCause?.Title ?? string.Empty, inputDto.When, personnelInfo.FullName, personnelInfo.MobileNumber, Environment.NewLine);
            string opLogText = string.Format(OpLogLiterals.ServiceLinkDisconnectInsertOpLog, inputDto.BillId, connectDisconnectCause?.Title ?? string.Empty);


            ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto> result = await _connectDisconnectCommandHandler.GetResult(customerInfo, inputDto.BillId, inputDto.Why, typeTitle, messageText, companyInfo.CompanyName, cancellationToken);
            ConnectDisconnectInsertDto connectDisconnectInsertDto = _connectDisconnectCommandHandler.GetConnectDisconnectInsertDto(customerInfo, inputDto.Why, inputDto.Description, companyInfo, personnelInfo, appUser, connectDisconnectCause?.Title ?? string.Empty, typeId, typeTitle);
            await ExecSql(connectDisconnectInsertDto, appUser, opLogText);

            return result;
        }
        public async Task ExecSql(ConnectDisconnectInsertDto connectDisconnectInsertDto, IAppUser appUser, string logTex)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    await _connectDisconnectCommandHandler.ExecSql(connection, transaction, connectDisconnectInsertDto, appUser, logTex);
                    transaction.Commit();
                }
            }
        }
    }
}
