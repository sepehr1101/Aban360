namespace Aban360.ReportPool.Domain.Features.Dashboard.Dtos
{
    public record TileScriptWithParamsInputDto
    {
        public int Id { get; set; }

        public string? FromDateJalali { get; set; }
        public string? ToDateJalali { get; set; }
    }
}
