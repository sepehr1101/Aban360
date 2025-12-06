using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class BedBesCreateHadler : IBedBesCreateHadler
    {
        private readonly IBedBesCommandService _bedBesCreateService;
        private readonly IConfiguration _configuration;
        public BedBesCreateHadler(
            IBedBesCommandService bedBesCreateService,
            IConfiguration configuration)
        {
            _bedBesCreateService = bedBesCreateService;
            _bedBesCreateService.NotNull(nameof(_bedBesCreateService));

            _configuration = configuration;
            _configuration.NotNull(nameof(_configuration));
        }

        public async Task Handle(AbBahaCalculationDetails inputDto, decimal codVas, CancellationToken cancellationToken)
        {
            int dayToPay = int.Parse(_configuration["Invoice:TimeToPay"]);


            BedBesCreateDto bedBesDto = new BedBesCreateDto();
            #region
            bedBesDto.Town = inputDto.Customer.ZoneId;
            bedBesDto.Radif = inputDto.Customer.Radif;
            bedBesDto.Eshtrak = inputDto.Customer.ReadingNumber;
            bedBesDto.Barge = 0;
            bedBesDto.PriNo = inputDto.MeterInfo.PreviousNumber;
            bedBesDto.TodayNo = 0;//from object
            bedBesDto.PriDate = inputDto.MeterInfo.PreviousDateJalali;
            bedBesDto.TodayDate = " ";//from object
            bedBesDto.AbonFas = (decimal)inputDto.AbonmanFazelabAmount;
            bedBesDto.FasBaha = (decimal)inputDto.FazelabAmount;
            bedBesDto.AbBaha = (decimal)inputDto.AbBahaAmount;
            bedBesDto.Ztadil = 0;
            bedBesDto.Masraf = inputDto.MeterInfo.PreviousNumber - 0;//current nubmer
            bedBesDto.Shahrdari = 0;
            bedBesDto.Modat = inputDto.Duration;
            bedBesDto.JalaseNo = 0;
            bedBesDto.DateBed = DateTime.Now.ToShortPersianDateString();
            bedBesDto.Mohlat = DateTime.Now.AddDays(-dayToPay).ToShortPersianDateString();
            bedBesDto.Baha = (decimal)GetSumAll(inputDto);
            bedBesDto.AbonAb = (decimal)inputDto.AbonmanAbAmount;
            bedBesDto.Pard = (decimal)GetPard(GetSumAll(inputDto));
            bedBesDto.Ghabs = " ";
            bedBesDto.CodVas = codVas;
            bedBesDto.Del = false;
            bedBesDto.Type = "1";
            bedBesDto.CodEnshab = inputDto.Customer.UsageId;
            bedBesDto.Enshab = inputDto.Customer.MeterDiameterId;
            bedBesDto.Elat = 0;
            bedBesDto.Ser = 0;
            bedBesDto.Serial = 0;
            bedBesDto.ZaribFasl = (decimal)inputDto.HotSeasonAbBahaAmount;
            bedBesDto.Ab10 = 0;
            bedBesDto.Ab20 = 0;
            bedBesDto.TedadVahd = inputDto.Customer.OtherUnit;
            bedBesDto.TedKhane = inputDto.Customer.HouseholdNumber;
            bedBesDto.TedadMas = inputDto.Customer.DomesticUnit;
            bedBesDto.TedadTej = inputDto.Customer.CommertialUnit;
            bedBesDto.NoeVa = inputDto.Customer.BranchType;
            bedBesDto.Jarime = 0;
            bedBesDto.Masjar = 0;
            bedBesDto.Sabt = 1;
            bedBesDto.Rate = (decimal)inputDto.MonthlyConsumption;
            bedBesDto.Operator = 0;
            bedBesDto.Mamor = 0;//from input,
            bedBesDto.TavizDate = "from input";
            bedBesDto.ZaribCntr = 0;
            bedBesDto.Zabresani = 0;
            bedBesDto.ZaribD = 0;
            bedBesDto.Masraf = 0;
            bedBesDto.KasrHa = (decimal)GetSumDiscount(inputDto);
            bedBesDto.FixMas = inputDto.Customer.ContractualCapacity;
            bedBesDto.ShGhabs1 = inputDto.Customer.BillId;
            bedBesDto.ShPard1 = "generate";
            bedBesDto.TabAbnA = 0;
            bedBesDto.TabAbnF = 0;
            bedBesDto.TabsFa = 0;
            bedBesDto.TabAbnF = 0;
            bedBesDto.TabsFa = 0;
            bedBesDto.TabAbnA = 0;
            bedBesDto.NewAb = 0;
            bedBesDto.NewFa = 0;
            bedBesDto.Bodjeh = (decimal)inputDto.BoodjePart1Amount;
            bedBesDto.Faz = inputDto.FazelabAmount > 0 ? true : false;
            bedBesDto.Group1 = 0;
            bedBesDto.MasFas = 0;
            bedBesDto.DateIns = "";
            bedBesDto.KhaliS = inputDto.Customer.EmptyUnit;
            bedBesDto.EdarehK = inputDto.Customer.IsSpecial;
            bedBesDto.Avarez = (decimal)inputDto.AvarezAmount;
            bedBesDto.TrackNumber = 0;//from input
            #endregion
            await _bedBesCreateService.Create(bedBesDto,(int)bedBesDto.Town);
        }
        private double GetSumDiscount(AbBahaCalculationDetails ss)
        {
            return ss.AbBahaDiscount +
                   ss.HotSeasonDiscount +
                   ss.FazelabDiscount;
        }
        private double GetSumAll(AbBahaCalculationDetails ss)
        {
            return ss.AbBahaAmount +
                   ss.HotSeasonAbBahaAmount +
                   ss.AbonmanAbAmount +
                   ss.FazelabAmount +
                   ss.BoodjePart1Amount +
                   ss.BoodjePart2Amount +
                   ss.AbBahaDiscount +
                   ss.HotSeasonDiscount +
                   ss.FazelabDiscount +
                   ss.AvarezAmount +
                   ss.JavaniAmount +
                   ss.MaliatAmount +
                   ss.AbonmanFazelabAmount;
        }
        private double GetPard(double number)
        {
            double mode = number % 1000;
            return number + (1000 - mode);
        }
    }
}
