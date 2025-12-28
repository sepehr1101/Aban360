namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record BedBesOutputDto
    {
        public int Id { get; set; }
        public decimal ZoneId { get; set; }
        public decimal CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public decimal PreviousNumber { get; set; }
        public decimal CurrentNumber { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public decimal Item1 { get; set; }//AbBaha
        public decimal Item2 { get; set; }//FasBaha
        public decimal Item3 { get; set; }//AbonAb
        public decimal Item4 { get; set; }//AbonFas
        public decimal Item5 { get; set; }//Shahrdari
        public decimal Item8 { get; set; }//Jarime
        public decimal Item9 { get; set; }//Zabresani
        public decimal Item10 { get; set; }//ZaribD
        public decimal Item11 { get; set; }//ZaribFasl
        public decimal Item12 { get; set; }//ztadil
        public decimal Item18 { get; set; }//Bodjeh
        public decimal Consumption { get; set; }
        public decimal Duration { get; set; }
        public string RegisterDateJalali { get; set; }
        public decimal Minutes { get; set; }
        public decimal SumItems { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal CounterStateCode { get; set; }
        public bool Removable { get; set; }
        public decimal UsageId { get; set; }//Code-ensh
        public decimal MeterDiameterId { get; set; }//enshab
        public decimal Cause { get; set; }
        public decimal BodySerial { get; set; }
        public decimal OtherUnit { get; set; }
        public decimal HouseholdNumber { get; set; }
        public decimal DomesticUnit { get; set; }
        public decimal CommertialUnit { get; set; }
        public decimal BranchType { get; set; }
        public decimal ConsumptionAverage { get; set; }
        public decimal Operator { get; set; }
        public string LastMeterChangeDateJalali { get; set; }//from input 
        public decimal Discount { get; set; }//kasrha
        public decimal ContractualCapacity { get; set; }
        public string BillId { get; set; }
        public string PayId { get; set; }
        public decimal UsageConsumption { get; set; }
        public bool HasSewage { get; set; }
        public decimal EmptyUnit { get; set; }
        public bool? IsSpecial { get; set; }
        public long TrackNumber { get; set; }
        //remove: Barge ,Type ,Masjar , Sabt ,mamor,ChkKarbari, Tafavot, c200,DateIns, AbSevom,AbSevom1, c70 , c80 ,c90 , c101 ,TmpDateBed ,TmpPriDate , TmpTodayDate, TmpMohlat, TmpTavizDate, Tafa402,Avarez

    }
}
