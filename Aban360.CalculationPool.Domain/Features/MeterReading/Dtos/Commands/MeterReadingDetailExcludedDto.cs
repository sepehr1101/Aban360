using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingDetailExcludedDto
    {
        public int Id { get; set; }
        public ExcludedCauseEnum ExcludedCauseId { get; set; }
        public string ExcludedCauseTitle { get; set; }
        public Guid ExcludedByUserId { get; set; }
        public DateTime ExcludedDateTime { get; set; }
        public MeterReadingDetailExcludedDto(int id, Guid excludedByUserId, DateTime excludedDateTime, ExcludedCauseEnum excludedCuaseId, string excludedCauseTitle)
        {
            Id = id;
            ExcludedByUserId = excludedByUserId;
            ExcludedDateTime = excludedDateTime;
            ExcludedCauseId = excludedCuaseId;
            ExcludedCauseTitle = excludedCauseTitle;
        }
    }
}
