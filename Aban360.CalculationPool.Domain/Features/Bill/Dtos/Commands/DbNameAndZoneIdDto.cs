namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record DbNameAndZoneIdDto
    {
        public string DbName { get; set; }
        public int ZoneId { get; set; }
        public DbNameAndZoneIdDto(string dbName, int zoneId)
        {
            DbName = dbName;
            ZoneId = zoneId;
        }
        public DbNameAndZoneIdDto()
        {
        }
    }
}
