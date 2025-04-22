using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class TestCalculationBatchHandler : ITestCalculationBatchHandler
    {
        public TestCalculationBatchHandler()
        {

        }
        public async Task<CaluclationIntervalDiscrepancytWrapper> Handle(CaluclationIntervalBatchTestInput testInput)
        {
            return new CaluclationIntervalDiscrepancytWrapper()
            {
                CurrentSystemSum = 1900045600,
                DifferenceSum = 56997,
                PreviousSystemSum = 1920659000,
                DiscrepancyDetails = new List<CaluclationIntervalDiscrepancy>()
                 {
                     new CaluclationIntervalDiscrepancy(){Amount=16000,BillId="12345600",CustomerNumber=4561,FromReadingDate="1403/09/12",FromWaterMeterNumber=56698,ToWaterMeterNumber=57800},
                     new CaluclationIntervalDiscrepancy(){Amount=19000,BillId="98765123",CustomerNumber=9361,FromReadingDate="1403/12/18",FromWaterMeterNumber=7805,ToWaterMeterNumber=7915}
                 }
            };
        }
    }
}
