using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class JudicialNoticeSetResultHandler : AbstractBaseConnection, IJudicialNoticeSetResultHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConnectDisconnectQueryService _connectDisconnectQueryService;
        private readonly IValidator<JudicalNoticeSetResultInputDto> _validator;
        public JudicialNoticeSetResultHandler(
           IHttpContextAccessor contextAccessor,
           IConnectDisconnectQueryService connectDisconnectQueryService,
           IValidator<JudicalNoticeSetResultInputDto> validator,
           IConfiguration configuration)
              : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _connectDisconnectQueryService = connectDisconnectQueryService;
            _connectDisconnectQueryService.NotNull(nameof(connectDisconnectQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(JudicalNoticeSetResultInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, cancellationToken);
            ConnectDisconnectGetDto judicialNoticeInfo = await _connectDisconnectQueryService.Get(inputDto.Id, ReportLiterals.JudicalNoticeId);
            ServiceLinkDisconnectResultDto resultInfo = GetJudicialReslt(inputDto.ResultId);
            ConnectDisconnectUpdateDto connectDiscnnectUpdateDto = new(inputDto.Id, appUser.UserId, resultInfo.Id, resultInfo.Title, inputDto.JudicialId, string.Join("_", new string[] { judicialNoticeInfo.Description ?? string.Empty, inputDto.Description ?? string.Empty }));
            string opLogText = string.Format(OpLogLiterals.JudicalNoticeSetResultOpLog, judicialNoticeInfo.BillId, resultInfo.Title, inputDto.JudicialId);

            await ExceSql(connectDiscnnectUpdateDto, appUser, opLogText);
        }
        private async Task ExceSql(ConnectDisconnectUpdateDto connectDisconnectIUpdateDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    ConnectDisconnectCommandService connectDisconnectCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await connectDisconnectCommandService.Update(connectDisconnectIUpdateDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task Validate(JudicalNoticeSetResultInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
        private ServiceLinkDisconnectResultDto GetJudicialReslt(int id)
        {
            ServiceLinkDisconnectResultDto? resultInfo = GetJudicalNoticeResults().Where(j => j.Id == id).FirstOrDefault();
            if (resultInfo is null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidId);
            }
            return resultInfo;
        }
        public ICollection<ServiceLinkDisconnectResultDto> GetJudicalNoticeResults()
        {
            ICollection<ServiceLinkDisconnectResultDto> results = new List<ServiceLinkDisconnectResultDto>()
            {
                new ServiceLinkDisconnectResultDto(1,"مشترک تسویه حساب نمود.",true),
                new ServiceLinkDisconnectResultDto(2,".مشترک هنوز بدهکار است",false),
            };
            return results;
        }
        public ICollection<NumericDictionary> GetCustomerDebtSubResult()
        {
            ICollection<NumericDictionary> results = new List<NumericDictionary>()
            {
                new NumericDictionary(1,"ثبت شماره بایگانی"),
                new NumericDictionary(2,"ثبت درخواست ممنوع الخروج"),
                new NumericDictionary(3,"ثبت درخواست بستن حسابهای بانکی"),
                new NumericDictionary(4,"ثبت درخواست توقیف اموال منقول وسایل نقلیه"),
                new NumericDictionary(5,"ثبت درخواست توقیف اموال غیرمنقول املاک"),
                new NumericDictionary(6,"ثبت درخواست قطع شماره تلفن و موبایل"),
            };
            return results;
        }
    }
}