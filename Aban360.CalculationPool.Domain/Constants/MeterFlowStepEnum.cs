namespace Aban360.CalculationPool.Domain.Constants
{
    public enum MeterFlowStepEnum:short
    {
        Imported=1,//merge 1 , 2
        Calculated=2,

        ConsumptionChecked=3,
        //AmountChecked=4,//removed
        CalculationConfirmed=5,
        ClientNotification=6
    }
}
