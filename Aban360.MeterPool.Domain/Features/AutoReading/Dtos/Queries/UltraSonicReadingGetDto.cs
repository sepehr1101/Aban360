using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aban360.MeterPool.Domain.Features.AutoReading.Dtos.Queries
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public record UltraSonicReadingGetDto
    {
        public long Id { get; set; }
        public string? PersianDateTimeReadAll { get; set; }
        public DateTime? GreDateTimeReadAll { get; set; }
        public string? ServerPersianDateTime { get; set; }
        public DateTime? GreServerDateTime { get; set; }
        public string? Imei { get; set; }
        public string? FirmwareVersion { get; set; }
        public string? BootLoaderVersion { get; set; }
        public string? HardwareVersion { get; set; }
        public decimal? BatteryVoltage { get; set; }
        public decimal? BackupBatteryVoltage { get; set; }
        public string? OwnerName { get; set; }
        public string? FileNumber { get; set; }
        public decimal? AntennaSignal { get; set; }
        public string? StartConnectOnlineTime { get; set; }
        public string? ConnectionTime { get; set; }
        public string? DomainName { get; set; }
        public int? PortNumber { get; set; }
        public bool? DstEnable { get; set; }
        public string? DstStartTime { get; set; }
        public string? DstEndTime { get; set; }
        public int? DstChangeAmount { get; set; }
        public int? SamplingInterval { get; set; }
        public int? OperationMode { get; set; }
        public decimal? InstantFlow { get; set; }
        public decimal? PositiveTotalConsumption { get; set; }
        public decimal? NegativeTotalConsumption { get; set; }
        public string? FlowMeterSerial { get; set; }
        public string? SmsCenterNumber { get; set; }
        public JsonElement? WirelessSimCard { get; set; }
        public string? WireLessVersion { get; set; }
        public decimal? ValveOpenPercent { get; set; }
        public int? CurrentValveState { get; set; }
        public int? CommunicationMethod { get; set; }
        public string? PersianOpenDoorDateTime { get; set; }
        public DateTime? GreOpenDoorDateTime { get; set; }
        public bool? OpenDoor { get; set; }
        public decimal? WaterTemperature { get; set; }
        public int? CalibFactorCrc { get; set; }
        public decimal? BatteryEsr { get; set; }
        public decimal? LeakDetectionValvePercent { get; set; }
        public JsonElement? ValveOperationFlag { get; set; }
        public decimal? EnvironmentTemperature { get; set; }
        // Only null values were supplied; preserve the provider's value until its schema is known.
        public JsonElement? Pressure { get; set; }
    }
}
