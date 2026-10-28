using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Constants;
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

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class ConnectCommandHandler : AbstractBaseConnection, IConnectCommandHandler
    {
        private readonly IConnectDisconnectCommandBaseHandler _connectDisconnectCommandHandler;
        private readonly IOtherExpensesInsertHandler _otherExpensesInsertCommandHandler;
        private int typeId = ReportLiterals.ConnectId;
        private string typeTitle = ReportLiterals.Connect;
        public ConnectCommandHandler(
            IConnectDisconnectCommandBaseHandler connectDisconnectCommandHandler,
            IOtherExpensesInsertHandler otherExpensesInsertCommandHandler,
            IConfiguration configuration)
                : base(configuration)
        {
            _connectDisconnectCommandHandler = connectDisconnectCommandHandler;
            _connectDisconnectCommandHandler.NotNull(nameof(connectDisconnectCommandHandler));

            _otherExpensesInsertCommandHandler = otherExpensesInsertCommandHandler;
            _otherExpensesInsertCommandHandler.NotNull(nameof(otherExpensesInsertCommandHandler));
        }

        public async Task<ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto>> Handle(ConnectPrintInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo = await _connectDisconnectCommandHandler.ValidateAndGetCustomerGeneral(inputDto.BillId, inputDto.Why, true);
            var (companyInfo, personnelInfo) = await _connectDisconnectCommandHandler.GetCompanyInfo(inputDto.CompanyId, inputDto.PersonnelId);
            NumericDictionary? connectDisconnectCause = _connectDisconnectCommandHandler.GetCasues().Where(c => c.Id == (inputDto.Why ?? 0)).FirstOrDefault();

            OtherExpensesInsertInputDto otherExpensesInputDto = new(inputDto.BillId, OtherExpensesEnum.ConnectDisconnect, inputDto.Amount);
            MemberInfoGetDto memberInfo = await _otherExpensesInsertCommandHandler.ValidateAndGetMemberInfo(otherExpensesInputDto, appUser);
            var (kartsInsertDto, requestBillDetailsInsertDto) = await _otherExpensesInsertCommandHandler.GetKartAndRequestBillDetailDto(otherExpensesInputDto, memberInfo);

            string messageText = string.Format(SmsTemplates.ServiceLinkConnectAlert, customerInfo.ReportData.FirstOrDefault().ZoneTitle, customerInfo.ReportHeader.FullName, customerInfo.ReportHeader.BillId, inputDto.When, personnelInfo.FullName, personnelInfo.MobileNumber, Environment.NewLine);
            string opLogText = string.Format(OpLogLiterals.ServiceLinkConnectByOfferingInsertOpLog, inputDto.BillId, inputDto.Amount, kartsInsertDto?.Sum(s => s.FinalAmount) ?? 0);

            ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto> result = await _connectDisconnectCommandHandler.GetResult(customerInfo, inputDto.BillId, inputDto.Why, typeTitle, messageText, companyInfo.CompanyName, cancellationToken);
            ConnectDisconnectInsertDto connectDisconnectInsertDto = _connectDisconnectCommandHandler.GetConnectDisconnectInsertDto(customerInfo, inputDto.Why, inputDto.Description, companyInfo, personnelInfo, appUser, connectDisconnectCause?.Title ?? string.Empty, typeId, typeTitle);

            await ExecSql(kartsInsertDto, requestBillDetailsInsertDto, connectDisconnectInsertDto, otherExpensesInputDto, memberInfo, appUser, opLogText);
            return result;
        }
        public async Task ExecSql(IEnumerable<KartInsertDto> kartsInsertDto, IEnumerable<RequestBillDetailsInsertDto> requestBillDetailsInsertDto, ConnectDisconnectInsertDto connectDisconnectInsertDto, OtherExpensesInsertInputDto otherExpensesInputDto, MemberInfoGetDto memberInfo, IAppUser appUser, string logTex)
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
                    await _otherExpensesInsertCommandHandler.ExecSql(connection, transaction, kartsInsertDto, requestBillDetailsInsertDto, otherExpensesInputDto, memberInfo, appUser, null);

                    transaction.Commit();
                }
            }
        }
    }
}