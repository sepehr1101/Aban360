namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsAssignUploadedFileInputDto
    {
        public string FileId { get; set; }
        public string FileYear { get; set; }
        public string FileCycle { get; set; }
        public string FileDescription { get; set; }
        public CollectBillsAssignUploadedFileInputDto(string fileId, string fileYear, string fileCycle, string fileDescription)
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
