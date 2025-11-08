namespace Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands
{
    public record SearchShortInputDto
    {
        public short Id { get; set; }

        public static implicit operator SearchShortInputDto(short id)
            => new SearchShortInputDto { Id = id };

        public static implicit operator short(SearchShortInputDto dto)
            => dto.Id;
    }
}
