using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IItemCalculator
    {
        double Calculate(string date1, string date2, int durationAll, CustomerInfoOutputDto customerInfo, double abBahaItemAmount, string currentDateJalali, bool isAbonman);
    }
}