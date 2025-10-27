namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record SaleOutputDto
    {
        public bool HasBroker { get; set; }
        public long CompanyAmount { get; set; }
        public long? BrokerAmount { get; set; }

        public long WaterInstallationAmount { get; set; }
        public long WaterEquipmentAmount { get; set; }
        public long Article11WaterMeterAmount { get; set; }
        public long Article11WaterAmount { get; set; }


        public long? SewageInstallationAmount { get; set; }
        public long? SewageEquipmentAmount { get; set; }
        public long? Article11SewageMeterAmount { get; set; }
        public long? Article11SewageAmount { get; set; }
    }
}
