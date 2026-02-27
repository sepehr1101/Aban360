namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingDetailDataOutputDto
    {
        public int Id { get; set; }
        public int FlowImportedId { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string BillId { get; set; }
        public int AgentCode { get; set; }
        public short CurrentCounterStateCode { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public int PreviousNumber { get; set; }
        public int CurrentNumber { get; set; }

        public Guid? ExcludedByUserId { get; set; }
        public DateTime? ExcludedDateTime { get; set; }

        public Guid InsertByUserId { get; set; }
        public DateTime InsertDateTime { get; set; }
        public Guid? RemovedByUserId { get; set; }
        public DateTime? RemovedDateTime { get; set; }

        public int BranchTypeId { get; set; }
        public string BranchTypeTitle { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int ConsumptionUsageId { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int EmptyUnit { get; set; }
        public string WaterInstallationDateJalali { get; set; }
        public string SewageInstallationDateJalali { get; set; }
        public string WaterRegisterDate { get; set; }
        public string SewageRegisterDate { get; set; }
        public int WaterCount { get; set; }
        public int SewageCalcState { get; set; }
        public int ContractualCapacity { get; set; }
        public int HouseholdNumber { get; set; }
        public string HouseholdDate { get; set; }
        public string? VillageId { get; set; }
        public bool IsSpecial { get; set; }
        public short MeterDiameterId { get; set; }
        public int VirtualCategoryId { get; set; }
        public string? BodySerial { get; set; }

        public string? TavizDateJalali { get; set; }
        public string? TavizCause { get; set; }
        public string? TavizRegisterDateJalali { get; set; }
        public int? TavizNumber { get; set; }

        //previous
        public string LastMeterDateJalali { get; set; }
        public int? LastMeterNumber { get; set; }
        public float? LastConsumption{ get; set; }
        public float? LastMonthlyConsumption { get; set; }
        public int? LastCounterStateCode { get; set; }
        public double? LastSumItems { get; set; }


        //current
        public double? SumItems { get; set; }
        public double? SumItemsBeforeDiscount { get; set; }
        public double? DiscountSum { get; set; }

        public double? Consumption { get; set; }
        public double? MonthlyConsumption { get; set; }



        //BedBes Props
        public decimal? Barge { get; set; }//
        public decimal? PriNo { get; set; }
        public decimal? TodayNo { get; set; }
        public string? PriDate { get; set; }
        public string? TodayDate { get; set; }
        public decimal? AbonFas { get; set; }
        public decimal? FasBaha { get; set; }
        public decimal? AbBaha { get; set; }
        public decimal? Ztadil { get; set; }
        public decimal? Masraf { get; set; }
        public decimal? Shahrdari { get; set; }//0
        public decimal? Modat { get; set; }
        public string? DateBed { get; set; }//emroz
        public decimal? JalaseNo { get; set; }//
        public string? Mohlat { get; set; }//10 rooz bad emroz ( from appsettirn)
        public decimal? Baha { get; set; }//sum all (dis ...)
        public decimal? AbonAb { get; set; }
        public decimal? Pard { get; set; }//ger baha -> 3 ragham akhar gerd
        public decimal? Jam { get; set; }//baha+ bedehi ghabli
        public decimal? CodVas { get; set; }//1--8 ->from inputDto
        public string? Ghabs { get; set; }//?
        public bool? Del { get; set; }//0
        public string? Type { get; set; }//1
        public decimal? CodEnshab { get; set; }//usageid
        public decimal? Enshab { get; set; }//meterDiameterId
        public decimal? Elat { get; set; }//0
        public decimal? Serial { get; set; }//
        public decimal? Ser { get; set; }//
        public decimal? ZaribFasl { get; set; }
        public decimal? Ab10 { get; set; }//0
        public decimal? Ab20 { get; set; }//0
        public decimal? TedadVahd { get; set; }//other
        public decimal? TedKhane { get; set; }
        public decimal? TedadMas { get; set; }
        public decimal? TedadTej { get; set; }
        public decimal? NoeVa { get; set; }//branch
        public decimal? Jarime { get; set; }//0
        public decimal? Masjar { get; set; }//0
        public decimal? Sabt { get; set; }//1
        public decimal? Rate { get; set; }//monthly avergecondmption 
        public decimal? Operator { get; set; }//0 
        public decimal? Mamor { get; set; }//input
        public string? TavizDate { get; set; }//from input
        public decimal? ZaribCntr { get; set; }//0
        public decimal? Zabresani { get; set; }//0
        public decimal? ZaribD { get; set; }//0
        public decimal? Tafavot { get; set; }//0
        public decimal? KasrHa { get; set; }//sum discount ha
        public decimal? FixMas { get; set; }//contr..
        public string? ShGhabs1 { get; set; }//billid
        public string? ShPard1 { get; set; }//must generte in c#
        public decimal? TabAbnA { get; set; }//0
        public decimal? TabAbnF { get; set; }//0
        public decimal? TabsFa { get; set; }//0
        public decimal? NewAb { get; set; }//0
        public decimal? NewFa { get; set; }//0
        public decimal? Bodjeh { get; set; }//bodje amount
        public decimal? Group1 { get; set; }//0
        public decimal? MasFas { get; set; }//0
        public bool? Faz { get; set; }//fazelab amount>0?1:0
        public decimal? ChkKarbari { get; set; }//0
        public decimal? C200 { get; set; }//0
        public decimal? AbSevom { get; set; }//0
        public decimal? AbSevom1 { get; set; }//0
        public decimal? C70 { get; set; }//0
        public decimal? C80 { get; set; }//0
        public decimal? C90 { get; set; }//0
        public decimal? C101 { get; set; }//0
        public decimal? KhaliS { get; set; }//empty unit
        public bool? EdarehK { get; set; }//isSpecial
        public decimal? Avarez { get; set; }//avarez amoutn

        //KasrHa Props
        public double AbBahaDiscount { get; set; }
        public double HotSeasonDiscount { get; set; }
        public double HotSeasonFazelabDiscount { get; set; }
        public double FazelabDiscount { get; set; }
        public double AbonmanAbDiscount { get; set; }
        public double AbonmanFazelabDiscount { get; set; }
        public double AvarezDiscount { get; set; }
        public double JavaniDiscount { get; set; }
        public double BoodjeDiscount { get; set; }
        public double MaliatDiscount { get; set; }

    }
}
