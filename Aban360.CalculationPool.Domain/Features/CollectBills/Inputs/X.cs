namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record X
    {
        public string Zone { get; set; }
        public string ZoneAddress { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string AgentCode { get; set; }
        public string ReadingZone { get; set; }
        public string Cycle { get; set; }
        public string BillKind { get; set; }
        public string IssueDate { get; set; }
        public string NextReadingDate { get; set; }
        public string BillSerialNumber { get; set; }
        public string SubscriptionNumber { get; set; }
        public string FileNumber { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string CounterSerialNumber { get; set; }
        public string CounterStatus { get; set; }

        public long ResidentialPurchased { get; set; }
        public long ResidentialOccupied { get; set; }
        public long NonResidentialPurchased { get; set; }
        public long FamilyCount { get; set; }

        public string Tariff { get; set; }
        public string WaterDiameter { get; set; }
        public string SewageDiameter { get; set; }

        public long Capacity { get; set; }

        public string PreviousReadingDate { get; set; }
        public string CurrentReadingDate { get; set; }

        public long Days { get; set; }
        public long PreviousCounterDigit { get; set; }
        public long CurrentCounterDigit { get; set; }

        public double Consumption { get; set; }
        public double AverageConsumption { get; set; }
        public double AllowedConsumption { get; set; }
        public double ExtraConsumption { get; set; }

        public long PreviousDebt { get; set; }
        public long BudgetLawToll { get; set; }
        public long WaterCostNote2 { get; set; }
        public long WaterCostNote3 { get; set; }
        public long WaterSubscription { get; set; }
        public long WaterCost { get; set; }
        public long WarmWaterCost { get; set; }
        public long ExtraWaterCost { get; set; }
        public long WaterSubscriptionNote3 { get; set; }
        public long WaterArticle7 { get; set; }

        public long SewageCost { get; set; }
        public long SewageSubscription { get; set; }
        public long SewageCostNote3 { get; set; }
        public long SewageSubscriptionNote3 { get; set; }
        public long SewageArticle7 { get; set; }

        public long ValueAddedTax { get; set; }

        public long WaterBranchInstallmentCost { get; set; }
        public long SewageInstallmentCost { get; set; }
        public long ServiceInstallmentCost { get; set; }
        public long WaterInstallmentCost { get; set; }

        public string OtherCostsDescription { get; set; }
        public long OtherCostsAmount { get; set; }

        public long InvoiceSum { get; set; }
        public long CurrentRounding { get; set; }
        public long Amount { get; set; }

        public string AmountString { get; set; }
        public string PaymentDate { get; set; }
        public string BillMessage { get; set; }

        public string BillID { get; set; }
        public string PaymentID { get; set; }
        public string Barcode { get; set; }

        public string MobileNumber { get; set; }
        public string NationalID { get; set; }

        public long Code4TariffL { get; set; }
        public long ProvinceCode { get; set; }
        public long TownshipCode { get; set; }
        public long SectionCode { get; set; }
        public long CityParishCode { get; set; }
        public long? VillageCode { get; set; }

        public long AreaCode { get; set; }
        public long BranchTypeCode { get; set; }
        public long CalculationTypeCode { get; set; }
        public long ServiceTypeCode { get; set; }
        public long BranchStatusCode { get; set; }
        public long ReadingStatusCode { get; set; }
        public long BillStatusCode { get; set; }
        public long ReadingTypeCode { get; set; }
        public long CounterStatusCode { get; set; }
        public long BillKindCode { get; set; }

        public double CityCoefficient { get; set; }

        public long CalcUnits { get; set; }
        public long SewageCapacity { get; set; }
        public long YouthPopulation { get; set; }

        public string Reserve1 { get; set; }
        public string Reserve2 { get; set; }
        public string Reserve3 { get; set; }

        public long Reserve4 { get; set; }
        public long Reserve5 { get; set; }
        public long Reserve6 { get; set; }

        public string AbfaBillLocationIdent { get; set; }
    }
}
