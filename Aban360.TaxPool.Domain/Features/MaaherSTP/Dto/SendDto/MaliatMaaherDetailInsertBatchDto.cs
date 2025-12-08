namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto
{
    public record MaliatMaaherDetailInsertBatchDto
    {
        public int WrapperId { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public MaliatMaaherDetailInsertBatchDto(int wrapperId, string fromDateJalali, string toDateJalali)
        {
            WrapperId = wrapperId;

            FromDateJalali = fromDateJalali;
            ToDateJalali = toDateJalali;
        }
    }
}
