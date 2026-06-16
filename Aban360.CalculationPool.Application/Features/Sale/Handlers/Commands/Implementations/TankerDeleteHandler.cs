using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Implementations
{
    internal sealed class TankerDeleteHandler : AbstractBaseConnection, ITankerDeleteHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IVariabService _variabService;
        private readonly ITankerQueryService _tankerQueryService;
        const int _operator = 666;
        public TankerDeleteHandler(
            IHttpContextAccessor contextAccessor,
            ICommonZoneService commonZoneService,
            IVariabService variabService,
            ITankerQueryService tankerQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));

            _tankerQueryService = tankerQueryService;
            _tankerQueryService.NotNull(nameof(tankerQueryService));
        }

        public async Task Handle(TankerDeleteInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _commonZoneService.IsUserInZone(appUser, inputDto.ZoneId);
            TankerOutputDto tankerInfo = await DateValidate(inputDto);
            string opLogText = string.Format(OpLogLiterals.TankerDeleteOpLog, appUser.Username, DateTime.Now.ToShortPersianDateString(), inputDto.ZoneId, tankerInfo.BillId, tankerInfo.CustomerNumber, tankerInfo.Amount);
            TankerDeleteDto tankerDeleteDto = new(inputDto.ZoneId, inputDto.CustomerNumber, _operator);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = new(tankerDeleteDto.ZoneId, tankerDeleteDto.CustomerNumber);
            await SqlCommands(tankerDeleteDto, zoneIdAndCustomerNumber, appUser, opLogText);
        }
        private async Task SqlCommands(TankerDeleteDto tankerDeleteDto, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, IAppUser appUser, string opLogText)
        {
            string dbName = GetDbName(tankerDeleteDto.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    TankerCommandService tankerCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                    BedBesCommandService bedBesCommandService = new(connection, transaction);
                    BillCommandService billCommandService = new(connection, transaction);

                    await tankerCommandService.Delete(tankerDeleteDto, dbName);
                    await bedBesCommandService.Delete(zoneIdAndCustomerNumber, dbName);
                    await billCommandService.Delete(zoneIdAndCustomerNumber);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task<TankerOutputDto> DateValidate(TankerDeleteInputDto inputDto)
        {
            string DateCheckVariab = await _variabService.GetDateCheck(inputDto.ZoneId);
            TankerOutputDto tankerInfo = await _tankerQueryService.Get(new ZoneIdAndCustomerNumber(inputDto.ZoneId, inputDto.CustomerNumber));
            if (!string.IsNullOrWhiteSpace(tankerInfo.PaymentDateJalali))
            {
                throw new TankerException(ExceptionLiterals.InvalidDeleteTankerAfterPaid);
            }
            if (tankerInfo.RegisterDateJalali.CompareTo(DateCheckVariab) < 0)
            {
                throw new TankerException(ExceptionLiterals.InvalidDeleteTankerAfterDateCheck);
            }

            return tankerInfo;
        }
    }
}
