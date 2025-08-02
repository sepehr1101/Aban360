namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record BaseOldTariffEngineImaginaryInputDto
    {
        public CustomerDetailInfoInputDto customerInfo { get; set; }
        public MeterInfoByPreviousDataInputDto MeterPreviousData { get; set; }

    }
}
