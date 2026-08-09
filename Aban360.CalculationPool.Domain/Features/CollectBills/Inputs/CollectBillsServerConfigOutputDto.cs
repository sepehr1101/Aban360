using Aban360.CalculationPool.Domain.Features.CollectBills.Outputs;

namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsServerConfigOutputDto
    {
        public IEnumerable<CollectBillsServerConfigParametersOutputDto> CityDictionary { get; set; }
        public IEnumerable<long> CyclesInYearForReportArray { get; set; }
        public IEnumerable<long> CyclesInYearForUploadArray { get; set; }
        public IEnumerable<CollectBillsServerConfigParametersOutputDto> PaymentBankDictionary { get; set; }
        public IEnumerable<CollectBillsServerConfigParametersOutputDto> PaymentChannelDictionary { get; set; }
        public IEnumerable<CollectBillsServerConfigParametersOutputDto> PaymentCompanyDictionary { get; set; }
        public IEnumerable<CollectBillsServerConfigParametersOutputDto> SystemLogActionDictionary { get; set; }
        public IEnumerable<CollectBillsServerConfigParametersOutputDto> TimePeriodsForReportsDictionary { get; set; }
        public IEnumerable<CollectBillsServerConfigParametersOutputDto> UserFriendlyBillConfirmationStatusDictionary { get; set; }
        public IEnumerable<CollectBillsServerConfigParametersOutputDto> UserFriendlyFileStatusDictionary { get; set; }
        public IEnumerable<CollectBillsServerConfigParametersOutputDto> UserRoleDictionary { get; set; }
        public IEnumerable<CollectBillsServerConfigParametersOutputDto> WaterCostDetailDictionary { get; set; }
        public IEnumerable<long> YearsForReportArray { get; set; }
        public IEnumerable<long> YearsForUploadArray { get; set; }
    }
}
