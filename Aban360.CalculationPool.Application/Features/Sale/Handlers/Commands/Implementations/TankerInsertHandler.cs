using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Data;
using Aban360.Common.BaseEntities;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Implementations
{
    internal sealed class TankerInsertHandler : AbstractBaseConnection, ITankerInsertHandler
    {
        private readonly IVariabService _variabService;
        private readonly ITankerWaterDistanceTariffQueryService _tankerQueryService;
        private readonly IZaribCQueryService _zaribCQueryService;
        private readonly IZaribGetService _zaribGetService;
        private readonly IT52QueryService _t52QueryService;
        static int _tankerWaterUsageId = 19;
        public TankerInsertHandler(
            IVariabService variabService,
            ITankerWaterDistanceTariffQueryService tankerQueryService,
            IZaribCQueryService zaribCQueryService,
            IZaribGetService zaribGetService,
            IT52QueryService t52QueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));

            _zaribCQueryService = zaribCQueryService;
            _zaribCQueryService.NotNull(nameof(zaribCQueryService));

            _zaribGetService = zaribGetService;
            _zaribGetService.NotNull(nameof(zaribGetService));

            _t52QueryService = t52QueryService;
            _t52QueryService.NotNull(nameof(t52QueryService));

            _tankerQueryService = tankerQueryService;
            _tankerQueryService.NotNull(nameof(tankerQueryService));
        }

        public async Task<TankerWaterCalculationOutputDto> Handle(TankerInsertInputDto inputDto, int userCode, CancellationToken cancellationToken)
        {
            TankerWaterCalculationOutputDto calcResult = await Calc(inputDto, cancellationToken);
            if (!inputDto.IsConfirm)
            {
                return calcResult;
            }
            string dbName = GetDbName(inputDto.ZoneId);
            decimal barge = await _variabService.GetAndRenew(inputDto.ZoneId);

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

                    int customerNumber = await variablesCommandService.GetAndRenewTankerRadif();
                    TankerInsertDto tankerInsertDto = GetTankerInsertDto(inputDto, calcResult, customerNumber, barge);
                    BedBesCreateDto bedBesInsertDto = await GetBedBesInsertDto(tankerInsertDto, calcResult, userCode);
                    calcResult.BillId = bedBesInsertDto.ShGhabs1;
                    calcResult.PaymentId = bedBesInsertDto.ShPard1;
                    calcResult.CustomerNumber= customerNumber;

                    await tankerCommandService.Insert(tankerInsertDto, dbName);
                    await bedBesCommandService.Insert(bedBesInsertDto, dbName);

                    transaction.Commit();
                }
            }
           
            return calcResult;
        }
        public async Task<TankerWaterCalculationOutputDto> Calc(TankerInsertInputDto input, CancellationToken cancellationToken)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();

            var (c, zb) = await GetZarib(input.ZoneId);
            decimal saleStateZarib = input.SaleState == TankerWaterSaleStateEnum.Nomads ? 1.5m : 4;

            long deliveryAmount = await CalcDeliveryAmount(input);
            decimal abBaha = (input.Consumption * saleStateZarib * c) * zb;
            decimal boodjeh = input.Consumption * 2000m;
            decimal multiplier = GetVarzaneMultiplier(input);

            return new TankerWaterCalculationOutputDto(null,null, null, abBaha * multiplier, boodjeh, deliveryAmount);
        }
        private decimal GetVarzaneMultiplier(TankerInsertInputDto input)
        {
            return input.SaleState != TankerWaterSaleStateEnum.Nomads && input.ZoneId == 133111 ? 0.5m : 1m;
        }
        private async Task<long> CalcDeliveryAmount(TankerInsertInputDto input)
        {
            if (input.SaleState != TankerWaterSaleStateEnum.WithTanker)
            {
                return 0;
            }

            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            TankerWaterDistanceTariffOutputDto tankerTariff = await _tankerQueryService.Get(input.Distance, currentDateJalali);
            return tankerTariff.Amount * input.Consumption;
        }
        private async Task<(int, decimal)> GetZarib(int zoneId)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();

            ZaribCQueryDto zaribC = await _zaribCQueryService.GetZaribC(currentDateJalali);
            ZaribGetDto zarib = await _zaribGetService.Get(zoneId, currentDateJalali);

            return (zaribC.C, zarib.Zb);
        }
        private TankerInsertDto GetTankerInsertDto(TankerInsertInputDto inputDto, TankerWaterCalculationOutputDto calcResult, int customerNumber, decimal barge)
        {
            return new TankerInsertDto()
            {
                ZoneId = inputDto.ZoneId,
                CustomerNumber = customerNumber,
                FirstName = inputDto.FirstName,
                Surname = inputDto.Surname??string.Empty,
                Address = inputDto.Address ?? string.Empty,
                Barge = (int)barge,
                Consumption = inputDto.Consumption,
                Amount = (long)calcResult.Final,
                IsNotShorb = true,//todo
                ReadingNumber = string.Empty,//todo
            };
        }
        private async Task<BedBesCreateDto> GetBedBesInsertDto(TankerInsertDto tankerInsertDto, TankerWaterCalculationOutputDto calcResult, int userCode)
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
                Shahrdari = 0,
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
                Type = "1",
                CodEnshab = _tankerWaterUsageId,
                Enshab = 1,
                Elat = 0,
                Serial = 0,
                Ser = 0,
                ZaribFasl = 0,
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
                Operator = userCode,
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
    }
}
