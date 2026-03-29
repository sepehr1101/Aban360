using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Implementations
{
    internal sealed class CalculationRequestHandler : AbstractBaseConnection, ICalculationRequestHandler
    {
        private readonly ISaleGetHandler _saleGetHandler;
        private readonly IAfterSaleGetHandler _afterSaleGetHandler;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private static int _saleServiceId = 1;
        private static int _afterSaleServiceId = 2;
        private static int _kartTypeId = 2;
        private static int _intervalDueDate = 30;
        private static string _insertBy = "Aban";

        public CalculationRequestHandler(
            ISaleGetHandler saleGetHandler,
            IAfterSaleGetHandler afterSaleGetHandler,
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            IConfiguration configuration)
        : base(configuration)
        {
            _saleGetHandler = saleGetHandler;
            _saleGetHandler.NotNull(nameof(saleGetHandler));

            _afterSaleGetHandler = afterSaleGetHandler;
            _afterSaleGetHandler.NotNull(nameof(afterSaleGetHandler));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
        }

        public async Task<ReportOutput<SaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>> Handle(int trackNumber, int userCode, CancellationToken cancellationToken)
        {
            //validation ->todo: whitch status enable?  
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();

            if (trackingInfo.ServiceGroupId == _saleServiceId)
            {
                ReportOutput<SaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto> result = await GetSaleResult(moshtrakInfo, cancellationToken);
                await Commands(result, moshtrakInfo, userCode, trackingInfo.BillId);
                return result;
            }
            else if (trackingInfo.ServiceGroupId == _afterSaleServiceId)
            {
                ReportOutput<SaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto> result = await GetAfterSaleResult(moshtrakInfo, cancellationToken);
                await Commands(result, moshtrakInfo, userCode, trackingInfo.BillId);
                return result;
            }

            throw new InvalidTrackingException(ExceptionLiterals.InvalidCalculation);
        }
        private async Task<ReportOutput<SaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>> GetSaleResult(MoshtrakOutputDto moshtrakInfo, CancellationToken cancellationToken)
        {
            SaleInputDto saleInputDto = new()
            {
                WaterDiameterId = (short)moshtrakInfo.MeterDiameterId,
                SiphonDiameterId = (short)moshtrakInfo.MainSiphon,
                ZoneId = moshtrakInfo.ZoneId,
                UsageId = moshtrakInfo.UsageId,
                Block = moshtrakInfo.BlockId,
                HasWaterBroker = moshtrakInfo.BrokerId == 0 ? false : true,
                ContractualCapacity = moshtrakInfo.ContractualCapacity,
                DomesticUnit = moshtrakInfo.DomesticUnit,
                CommertialUnit = moshtrakInfo.CommercialUnit,
                OtherUnit = moshtrakInfo.OtherUnit,
                DiscountCount = moshtrakInfo.DiscountCount,
                DiscountTypeId = moshtrakInfo.DiscountTypeId,
                IsSewageDiscount = moshtrakInfo.s35 == 1 ? true : false,
                IsWaterDiscount = moshtrakInfo.s34 == 1 ? true : false,
                HasWaterArticle11 = false,//todo
                HasSewageArticle11 = false,//todo
            };
            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> saleResult = await _saleGetHandler.Handle(saleInputDto, cancellationToken);
            ReportOutput<SaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto> result = new(saleResult.Title, saleResult.ReportHeader, GetSaleResult(saleResult.ReportData));
            return result;
        }
        private async Task<ReportOutput<SaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>> GetAfterSaleResult(MoshtrakOutputDto moshtrakInfo, CancellationToken cancellationToken)
        {
            MoshtrakServiceDto moshtrakServiceSelected = GetMoshtrakService(moshtrakInfo);
            MemberInfoGetDto previousMoshtrakInfo = await _commonMemberQueryService.Get(new ZoneIdAndCustomerNumber(moshtrakInfo.ZoneId, moshtrakInfo.CustomerNumber));

            AfterSaleItemsInputDto previousInfo = new()
            {
                WaterDiameterId = (short)previousMoshtrakInfo.MeterDiameterId,
                SiphonDiameterId = short.Parse(previousMoshtrakInfo.MainSiphon),
                UsageId = previousMoshtrakInfo.UsageId,
                HasWaterBroker = moshtrakInfo.BrokerId == 0 ? false : true,
                ContractualCapacity = previousMoshtrakInfo.ContractualCapacity,
                DomesticUnit = previousMoshtrakInfo.DomesticUnit,
                CommertialUnit = previousMoshtrakInfo.CommercialUnit,
                OtherUnit = previousMoshtrakInfo.OtherUnit,
                IsSewageDiscount = moshtrakInfo.s35 == 1 ? true : false,
                IsWaterDiscount = moshtrakInfo.s34 == 1 ? true : false,
                DiscountCount = 0,//todo,
                DiscountTypeId = 0,//todo,
            };
            AfterSaleItemsInputDto currentInfo = new()
            {
                WaterDiameterId = (short)moshtrakInfo.MeterDiameterId,
                SiphonDiameterId = (short)moshtrakInfo.MainSiphon,
                UsageId = moshtrakInfo.UsageId,
                HasWaterBroker = moshtrakInfo.BrokerId == 0 ? false : true,
                ContractualCapacity = moshtrakInfo.ContractualCapacity,
                DomesticUnit = moshtrakInfo.DomesticUnit,
                CommertialUnit = moshtrakInfo.CommercialUnit,
                OtherUnit = moshtrakInfo.OtherUnit,
                IsSewageDiscount = moshtrakInfo.s35 == 1 ? true : false,
                IsWaterDiscount = moshtrakInfo.s34 == 1 ? true : false,
                DiscountCount = moshtrakInfo.DiscountCount,
                DiscountTypeId = moshtrakInfo.DiscountTypeId,
            };
            AfterSaleInputDto afterSaleInputDto = new()
            {
                ZoneId = moshtrakInfo.ZoneId,
                Block = moshtrakInfo.BlockId,
                HasWaterArticle11 = false,//todo
                HasSewageArticle11 = false,//todo
                CompanyServiceIds = MoshtrakService.GetServicesSelected(moshtrakServiceSelected),
                PreviousData = previousInfo,
                CurrentData = currentInfo,
            };
            //AfterSaleCompanyServiceEnum
            //todo: not working
            FlatReportOutput<SaleHeaderOutputDto, AfterSaleDataOutputDto> afterSaleResult = await _afterSaleGetHandler.Handle(afterSaleInputDto, cancellationToken);
            ReportOutput<SaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto> result = new(afterSaleResult.Title, afterSaleResult.ReportHeader, GetAfterSaleResult(afterSaleResult.ReportData));
            return result;
        }
        private MoshtrakServiceDto GetMoshtrakService(MoshtrakOutputDto serviceSelected)
        {
            return new MoshtrakServiceDto()
            {
                s0 = serviceSelected.s0,
                s1 = serviceSelected.s1,
                s2 = serviceSelected.s2,
                s3 = serviceSelected.s3,
                s4 = serviceSelected.s4,
                s5 = serviceSelected.s5,
                s8 = serviceSelected.s8,
                s9 = serviceSelected.s9,
                s10 = serviceSelected.s10,
                s11 = serviceSelected.s11,
                s12 = serviceSelected.s12,
                s13 = serviceSelected.s13,
                s14 = serviceSelected.s14,
                s15 = serviceSelected.s15,
                s16 = serviceSelected.s16,
                s17 = serviceSelected.s17,
                s18 = serviceSelected.s18,
                s19 = serviceSelected.s19,
                s20 = serviceSelected.s20,
                s21 = serviceSelected.s21,
                s22 = serviceSelected.s22,
                s23 = serviceSelected.s23,
                s24 = serviceSelected.s24,
                s25 = serviceSelected.s25,
                s26 = serviceSelected.s26,
                s27 = serviceSelected.s27,
                s28 = serviceSelected.s28,
                s29 = serviceSelected.s29,
                s30 = serviceSelected.s30,
                s31 = serviceSelected.s31,
                s32 = serviceSelected.s32,
                s33 = serviceSelected.s33,
                s34 = serviceSelected.s34,
                s35 = serviceSelected.s35,
                s36 = serviceSelected.s36,
                s37 = serviceSelected.s37,
                s38 = serviceSelected.s38,
                s39 = serviceSelected.s39,
                s40 = serviceSelected.s40,
                s41 = serviceSelected.s41,
                s42 = serviceSelected.s42,
                s43 = serviceSelected.s43,
                s44 = serviceSelected.s44,
                s45 = serviceSelected.s45,
                s46 = serviceSelected.s46,
                s47 = serviceSelected.s47,
                s48 = serviceSelected.s48,
            };
        }
        private IEnumerable<SaleAndAfterSaleDataOutputDto> GetSaleResult(IEnumerable<SaleDataOutputDto> data) => data.Select(s => new SaleAndAfterSaleDataOutputDto(s.Id, s.Title, s.Amount, s.Discount, s.FinalAmount, s.DiscountTypeId, false));
        private IEnumerable<SaleAndAfterSaleDataOutputDto> GetAfterSaleResult(AfterSaleDataOutputDto data) => data.DifferentValue.Select(s => new SaleAndAfterSaleDataOutputDto(s.Id, s.Title, s.Amount, s.Discount, s.FinalAmount, s.DiscountTypeId, false));
        private async Task Commands(ReportOutput<SaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto> input, MoshtrakOutputDto moshtrakInfo, int userCode, string billId)
        {
            ICollection<KartInsertDto> kartsInsertDto = GetKartsInsertDto(input.ReportHeader, input.ReportData, moshtrakInfo, userCode);
            GhestInsertDto ghestInsertDto = GetGhestInsertDto(input.ReportHeader, input.ReportData, moshtrakInfo, billId);
            string dbName = GetDbName(moshtrakInfo.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    KartCommandService kartCommandService = new(connection, transaction);
                    GhestCommandService ghestCommandService = new(connection, transaction);

                    await kartCommandService.Insert(kartsInsertDto, dbName);
                    await ghestCommandService.Insert(ghestInsertDto, dbName);

                    transaction.Commit();
                }
            }
        }
        private ICollection<KartInsertDto> GetKartsInsertDto(SaleHeaderOutputDto header, IEnumerable<SaleAndAfterSaleDataOutputDto> datas, MoshtrakOutputDto moshtrakInfo, int userCode)
        {
            int[] disallowedInsertCategory = { (int)OfferingEnum.WaterEquipment };

            return datas
                .Where(s => moshtrakInfo.BrokerId == 0 ||  !disallowedInsertCategory.Contains(s.Id))
                .Select(s => new KartInsertDto()
                {
                    ZoneId = moshtrakInfo.ZoneId,
                    CustomerNumber = moshtrakInfo.CustomerNumber,
                    ReadingNumber = moshtrakInfo.ReadingNumber,
                    StringTrackNumber = moshtrakInfo.TrackNumber.ToString().PadLeft(11, '0'),
                    Serial = 0,
                    Barge = 0,
                    CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                    DueDateJalali = DateTime.Now.AddMonths(1).ToShortPersianDateString(),
                    DiscountTypeId = s.DiscountTypeId,
                    FinalAmount = s.FinalAmount,
                    DiscountAmount = s.Discount ?? 0,
                    PardN = s.FinalAmount,
                    PardG = 0,
                    Sum = s.FinalAmount,
                    ServiceSelectedId = s.Id,//todo: check has same id
                    SiphonId = moshtrakInfo.MainSiphon,
                    UsageId = moshtrakInfo.UsageId,
                    IsRegister = false,
                    TotalServicesAmount = header.CompanyFinalAmount,
                    FirstInstallment = s.FinalAmount,
                    JGEST_FA = 0,
                    PishFa = 0,
                    InstallmentPercent = 100,
                    Operator = userCode,
                    DomesticUnit = moshtrakInfo.DomesticUnit,
                    CommercialUnit = moshtrakInfo.CommercialUnit,
                    OtherUnit = moshtrakInfo.OtherUnit,
                    KartTypeId = _kartTypeId,//todo,
                    InsertedBy = _insertBy,
                    BankDateJalali = string.Empty,
                    Installment = 0,//todo
                    InstallmentCount = 1,
                    MeterDiameterId = moshtrakInfo.MeterDiameterId,
                    Ser = 0,
                    Type = 0,//todo
                })
            .ToList();
        }
        private GhestInsertDto GetGhestInsertDto(SaleHeaderOutputDto header, IEnumerable<SaleAndAfterSaleDataOutputDto> datas, MoshtrakOutputDto m, string billId)
        {
            if (string.IsNullOrWhiteSpace(billId))
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidBillId);
            }
            return new GhestInsertDto()
            {
                ZoneId = m.ZoneId,
                CustomerNumber = m.CustomerNumber,
                StringTrackNumber = m.TrackNumber.ToString().PadLeft(11, '0'),
                Identify = 0,//todo
                Cod1 = 0,
                Cod2 = 0,
                Cod3 = 0,
                Barge = 0,//todo
                Payable = header.CompanyFinalAmount,
                Type = 0,//todo
                InstallmentNumber = 0,
                CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                DueDateJalali = DateTime.Now.AddDays(_intervalDueDate).ToShortPersianDateString(),
                InsertBy = _insertBy,
                BillId = billId ?? string.Empty,//todo:remove
                PaymentId = TransactionIdGenerator.GeneratePaymentId(header.CompanyFinalAmount, billId)
            };
        }
    }
}
