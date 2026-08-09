namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsAssignUploadedFileInputDto
    {
        public long FileId { get; set; }
        public long FileYear { get; set; }
        public long FileCycle { get; set; }
        public string FileDescription { get; set; }
        public CollectBillsAssignUploadedFileInputDto(long fileId, long fileYear, long fileCycle, string fileDescription)
        {
            FileId = fileId;
            FileYear = fileYear;
            FileCycle = fileCycle;
            FileDescription = fileDescription;
        }
        public CollectBillsAssignUploadedFileInputDto()
        {
        }
    }
}
