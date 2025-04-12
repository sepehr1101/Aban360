namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record WaterInvoiceDto
    {
        public string Headquarters { get; set; }
        public string EconomicalNumber { get; set; }

        public string ZoneTitle { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }

        public string UsageConsumptionTitle { get; set; }
        public string UsageSellTitle { get; set; }

        public short UnitDomesticWater { get; set; }
        public short NoneDomestic { get; set; }
        public short EmptyUnit { get; set; }

        public short UsageId { get; set; }
        public string BodySerial { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string SiphonDiameterTitle { get; set; }
        public string CounterTitle { get; set; }

        public int ConsumerNumber { get; set; }
        public string ReadingNumber { get; set; }

        public string CurrentMeterDateJalali { get; set; }
        public string PreviousMeterDateJalali { get; set; }

        public int Duration { get; set; }

        public int CurrentMeterNumber { get; set; }
        public int PreviousMeterNumber { get; set; }

        public int ConsumptionM3 { get; set; }
        public int ConsumptionLiter { get; set; }
        public int ConsumptionAverage { get; set; }

        public ICollection<LineItemsDto> LineItems { get; set; } = new List<LineItemsDto>();

        public long Sum { get; set; }
        public long DisCount { get; set; }
        public long DebtorOrCreditorAmount { get; set; }

        public long PayableAmount { get; set; }
        public string PaymentDeadline { get; set; }

        public int ConsumptionState { get; set; }

        public ICollection<PreviousConsumptionsDto> PreviousConsumptions { get; set; }

        public string BillId { get; set; }
        public string PayId { get; set; }

        public string BarCode { get; set; }

        public string PaymenetAmountText { get; set; }

        public bool IsPayed { get; set; }
        public string Description { get; set; }
        public string PaymentDateJalali { get; set; }
        public string PaymentMethod { get; set; }

    }
}
