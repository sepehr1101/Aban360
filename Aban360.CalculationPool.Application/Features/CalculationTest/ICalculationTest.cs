namespace Aban360.CalculationPool.Application.Features.CalculationTest
{
    public interface ICalculationTest
    {
        string Handle<D>(string formula, D entity);
    }
}
