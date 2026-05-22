namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record BillInsertDto
    {
        //public long Id { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }

        public decimal CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string ReadingNumber { get; set; }

        public int PreviousNumber { get; set; }
        public int NextNumber { get; set; }

        public string PreviousDay { get; set; }
        public string NextDay { get; set; }
        public string RegisterDay { get; set; }

        public DateTime RegisterDayGregorian { get; set; }

        public string CounterStateTitle { get; set; }

        public decimal UsageId { get; set; }
        public decimal? UsageId2 { get; set; }

        public string UsageTitle { get; set; }
        public string? UsageTitle2 { get; set; }

        public string BranchType { get; set; }

        public decimal WaterDiameterId { get; set; }
        public string WaterDiameterTitle { get; set; }

        public decimal Siphon100 { get; set; }
        public decimal Siphon125 { get; set; }
        public decimal Siphon150 { get; set; }
        public decimal Siphon200 { get; set; }
        public decimal Siphon5 { get; set; }
        public decimal Siphon6 { get; set; }
        public decimal Siphon7 { get; set; }
        public decimal Siphon8 { get; set; }

        public decimal ContractCapacity { get; set; }

        public decimal DomesticCount { get; set; }
        public decimal CommercialCount { get; set; }
        public decimal OtherCount { get; set; }
        public decimal EmptyCount { get; set; }

        public int Consumption { get; set; }
        public int Duration { get; set; }

        public float ConsumptionAverage { get; set; }

        public string Deadline { get; set; }

        public long PreDebt { get; set; }

        public long Item1 { get; set; }
        public long Item2 { get; set; }
        public long Item3 { get; set; }
        public long Item4 { get; set; }
        public long Item5 { get; set; }
        public long Item6 { get; set; }
        public long Item7 { get; set; }
        public long Item8 { get; set; }
        public long Item9 { get; set; }
        public long Item10 { get; set; }
        public long Item11 { get; set; }
        public long Item12 { get; set; }
        public long Item13 { get; set; }
        public long Item14 { get; set; }
        public long Item15 { get; set; }
        public long Item16 { get; set; }
        public long Item17 { get; set; }
        public long Item18 { get; set; }

        public long SumItems { get; set; }
        public long Payable { get; set; }

        public string TypeId { get; set; }

        public long ItemOff1 { get; set; }
        public long ItemOff2 { get; set; }
        public long ItemOff3 { get; set; }
        public long ItemOff4 { get; set; }
        public long ItemOff5 { get; set; }
        public long ItemOff6 { get; set; }
        public long ItemOff7 { get; set; }
        public long ItemOff8 { get; set; }
        public long ItemOff9 { get; set; }
        public long ItemOff10 { get; set; }
        public long ItemOff11 { get; set; }
        public long ItemOff12 { get; set; }
        public long ItemOff13 { get; set; }
        public long ItemOff14 { get; set; }
        public long ItemOff15 { get; set; }
        public long ItemOff16 { get; set; }
        public long ItemOff17 { get; set; }
        public long ItemOff18 { get; set; }

        public bool IsFree { get; set; }

        public string? VillageId { get; set; }
        public string? VillageName { get; set; }

        public string? ZoneId2 { get; set; }
        public string? ReadingStateTitle { get; set; }
        public string? PayId { get; set; }

        public int? CounterStateCode { get; set; }
        public int? TypeCode { get; set; }
        public string? TypeTitle { get; set; }

        public int? ReturnCauseId { get; set; }
        public string? ReturnCauseTitle { get; set; }

        public int? BranchTypeId { get; set; }

        public bool IsSettlement { get; set; }
    }
}
