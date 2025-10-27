namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record DeleteDto
    {
        public short Id { get; set; }
        public string RemovedDateJalali { get; set; }
        public DeleteDto(short id,string removedDateJalali)
        {
            Id=id;
            RemovedDateJalali=removedDateJalali;
        }
    }
}
