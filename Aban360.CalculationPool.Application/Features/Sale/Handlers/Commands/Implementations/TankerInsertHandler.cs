using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Data;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Microsoft.AspNetCore.Http;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Aban360.Common.Db.Services;
using Aban360.OldCalcPool.Application.Constant;
using Aban360.Common.ApplicationUser;
using Aban360.CalculationPool.Application.Features.Base;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Implementations
{
    internal sealed class TankerInsertHandler : AbstractBaseConnection, ITankerInsertHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IVariabService _variabService;
        private readonly ITankerWaterDistanceTariffQueryService _tankerQueryService;
        private readonly IZaribCQueryService _zaribCQueryService;
        private readonly IZaribGetService _zaribGetService;
        private readonly IT52QueryService _t52QueryService;
        private readonly IT51QueryService _zoneQueryService;
        static int _tankerWaterUsageId = 19;
        static int _operator = 666;
        static int _typeId = 1;
        public TankerInsertHandler(
            IHttpContextAccessor contextAccessor,
            IVariabService variabService,
            ITankerWaterDistanceTariffQueryService tankerQueryService,
            IZaribCQueryService zaribCQueryService,
            IZaribGetService zaribGetService,
            IT52QueryService t52QueryService,
            IT51QueryService zoneQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));

            _zaribCQueryService = zaribCQueryService;
            _zaribCQueryService.NotNull(nameof(zaribCQueryService));

            _zaribGetService = zaribGetService;
            _zaribGetService.NotNull(nameof(zaribGetService));

            _t52QueryService = t52QueryService;
            _t52QueryService.NotNull(nameof(t52QueryService));

            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));

            _tankerQueryService = tankerQueryService;
            _tankerQueryService.NotNull(nameof(tankerQueryService));
        }

        public async Task<TankerCalculationResultOutputDto> Handle(TankerInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrimInputProp(inputDto);
            TankerCalculationBaseService tankerService = new TankerCalculationBaseService(_tankerQueryService, _zaribCQueryService, _zaribGetService);
            TankerWaterCalculationOutputDto calcResult = await tankerService.Calculate(GetTankerServiceDto(inputDto), inputDto.MobileNumber);
            if (!inputDto.IsConfirm)
            {
                return await GetResult(calcResult, inputDto);
            }
            decimal barge = await _variabService.GetAndRenew(inputDto.ZoneId);
            await SqlCommands(inputDto, calcResult, barge, appUser);

            return await GetResult(calcResult, inputDto);
        }
        private async Task SqlCommands(TankerInsertInputDto inputDto, TankerWaterCalculationOutputDto calcResult, decimal barge, IAppUser appUser)
        {
            string dbName = GetDbName(inputDto.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    VariablesCommandService variablesCommandService = new(connection, transaction);
                    TankerCommandService tankerCommandService = new(connection, transaction);
                    BedBesCommandService bedBesCommandService = new(connection, transaction);
                    BillCommandService billCommandService = new(connection, transaction);
                    OpLogCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    int customerNumber = await variablesCommandService.GetAndRenewTankerRadif();
                    TankerInsertDto tankerInsertDto = GetTankerInsertDto(inputDto, calcResult, customerNumber, barge);
                    BedBesCreateDto bedBesInsertDto = await GetBedBesInsertDto(tankerInsertDto, calcResult);
                    calcResult.BillId = bedBesInsertDto.ShGhabs1;
                    calcResult.PaymentId = bedBesInsertDto.ShPard1;
                    calcResult.CustomerNumber = customerNumber;
                    string opLogText = string.Format(Literals.TankerInsertOpLog, appUser.Username, tankerInsertDto.CurrentDateJalali, inputDto.ZoneId, bedBesInsertDto.ShGhabs1, tankerInsertDto.CustomerNumber, calcResult.Final);

                    await tankerCommandService.Insert(tankerInsertDto, dbName);
                    int bedBesId = await bedBesCommandService.Insert(bedBesInsertDto, dbName);
                    BillByBedBedIdInsertDto billByBedBesIdDto = new(tankerInsertDto.ZoneId, tankerInsertDto.CustomerNumber, _typeId, bedBesId);
                    await billCommandService.InsertByBedBesId(billByBedBesIdDto, dbName);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private TankerWaterCalculationInputDto GetTankerServiceDto(TankerInsertInputDto inputDto)
        {
            return new TankerWaterCalculationInputDto()
            {
                Consumption = inputDto.Consumption,
                Distance = inputDto.Distance,
                IsConfirm = inputDto.IsConfirm,
                SaleState = inputDto.SaleState,
                ZoneId = inputDto.ZoneId
            };
        }
        private TankerInsertDto GetTankerInsertDto(TankerInsertInputDto inputDto, TankerWaterCalculationOutputDto calcResult, int customerNumber, decimal barge)
        {
            return new TankerInsertDto()
            {
                ZoneId = inputDto.ZoneId,
                CustomerNumber = customerNumber,
                FirstName = inputDto.FirstName,
                Surname = inputDto.Surname ?? string.Empty,
                Address = inputDto.Address ?? string.Empty,
                Barge = (int)barge,
                Consumption = inputDto.Consumption,
                Amount = (long)calcResult.Final,
                IsNotShorb = true,//todo
                ReadingNumber = string.Empty,//todo
            };
        }
        private async Task<BedBesCreateDto> GetBedBesInsertDto(TankerInsertDto tankerInsertDto, TankerWaterCalculationOutputDto calcResult)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            //string _3digitZoneId = await _t52QueryService.Get(new ZoneIdAndCustomerNumber(tankerInsertDto.ZoneId, tankerInsertDto.CustomerNumber));
            string _3digitZoneId = "000";
            string billId = TransactionIdGenerator.GenerateBillId(tankerInsertDto.CustomerNumber.ToString(), _3digitZoneId);
            string payId = TransactionIdGenerator.GeneratePaymentId((long)calcResult.Final, billId, "100");

            return new BedBesCreateDto()
            {
                Town = tankerInsertDto.ZoneId,
                Radif = tankerInsertDto.CustomerNumber,
                Eshtrak = tankerInsertDto.ReadingNumber ?? string.Empty,
                Barge = tankerInsertDto.Barge,
                PriNo = 0,
                TodayNo = 0,
                PriDate = currentDateJalali,
                TodayDate = currentDateJalali,
                AbonFas = 0,
                FasBaha = 0,
                AbBaha = calcResult.Water,
                Ztadil = 0,
                Masraf = tankerInsertDto.Consumption,
                Shahrdari = calcResult.Tax,
                Modat = 30,
                DateBed = currentDateJalali,
                JalaseNo = 0,
                Mohlat = currentDateJalali,
                Baha = calcResult.Final,
                AbonAb = 0,
                Pard = calcResult.Final,
                Jam = calcResult.Final,
                CodVas = 0,
                Ghabs = string.Empty,
                Del = false,
                Type = _typeId.ToString(),
                CodEnshab = _tankerWaterUsageId,
                Enshab = 1,
                Elat = 0,
                Serial = 0,
                Ser = 0,
                ZaribFasl = calcResult.HotSeason,
                Ab10 = 0,
                Ab20 = 0,
                TedadVahd = 1,
                TedKhane = 0,
                TedadMas = 0,
                TedadTej = 0,
                NoeVa = 0,
                Jarime = 0,
                Masjar = 0,
                Sabt = 0,
                Rate = 30,
                Operator = _operator,
                Mamor = 0,
                TavizDate = string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = 0,
                Tafavot = 0,
                KasrHa = 0,
                FixMas = 0,
                ShGhabs1 = billId,
                ShPard1 = payId,
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                NewAb = 0,
                NewFa = 0,
                Bodjeh = calcResult.Budget,
                Group1 = 0,
                MasFas = 0,
                Faz = false,
                ChkKarbari = 0,
                C200 = 0,
                DateIns = string.Empty,
                AbSevom = 0,
                AbSevom1 = 0,
                C70 = 0,
                C80 = 0,
                TmpDateBed = string.Empty,
                TmpPriDate = string.Empty,
                TmpTodayDate = string.Empty,
                TmpMohlat = string.Empty,
                TmpTavizDate = string.Empty,
                C90 = 0,
                C101 = 0,
                KhaliS = 0,
                EdarehK = false,
                Tafa402 = 0,
                Avarez = 0,
                TrackNumber = 0,//todo
            };
        }
        private async Task<TankerCalculationResultOutputDto> GetResult(TankerWaterCalculationOutputDto calcResult, TankerInsertInputDto inputDto)
        {
            return new TankerCalculationResultOutputDto()
            {
                CustomerNumber = calcResult.CustomerNumber,
                BillId = calcResult.BillId,
                PaymentId = calcResult.PaymentId,
                MobileNumber = calcResult.MobileNumber,
                Water = calcResult.Water,
                Delivery = calcResult.Delivery,
                Budget = calcResult.Budget,
                ZoneId = inputDto.ZoneId,
                ZoneTitle = await _zoneQueryService.Get(inputDto.ZoneId),
                FirstName = inputDto.FirstName,
                Surname = inputDto.Surname,
                SaleStateTitle = GetSaleStateTitle(inputDto.SaleState),
                Consumption = inputDto.Consumption,
            };
        }
        private string GetSaleStateTitle(TankerWaterSaleStateEnum saleState)
        {
            return saleState switch
            {
                TankerWaterSaleStateEnum.WithTanker => "با تانکر",
                TankerWaterSaleStateEnum.WithoutTanker => "بدون تانکر",
                TankerWaterSaleStateEnum.Nomads => "به عشایر",
                _ => string.Empty,
            };
        }
        private void TrimInputProp(TankerInsertInputDto inputDto)
        {
            inputDto.FirstName = inputDto.FirstName.Trim();
            inputDto.Surname = inputDto?.Surname?.Trim() ?? string.Empty;
            inputDto.Address = inputDto?.Address?.Trim() ?? string.Empty;
        }
    }
}
