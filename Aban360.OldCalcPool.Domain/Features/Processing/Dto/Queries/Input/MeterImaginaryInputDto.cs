namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record MeterImaginaryInputDto
    {
        public CustomerDetailInfoInputDto? CustomerInfo { get; set; }
        public MeterInfoByPreviousDataInputDto? MeterPreviousData { get; set; }
    }
}
