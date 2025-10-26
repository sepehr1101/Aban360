namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record BaseOldTariffEngineImaginaryInputDto
    {
        public CustomerDetailInfoInputDto? CustomerInfo { get; set; }
        public MeterInfoByPreviousDataInputDto? MeterPreviousData { get; set; }
    }
}
