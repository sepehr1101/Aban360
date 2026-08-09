namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsUploadInputDto
    {
        public string Base64String { get; set; }
        public string CompressedFileExtension { get; set; }
        public string CompressedFileName { get; set; }
        public CollectBillsUploadInputDto(string base64String, string compressedFileExtension, string compressedFileName)
        {
            Base64String = base64String;
            CompressedFileExtension = compressedFileExtension;
            CompressedFileName = compressedFileName;
        }
        public CollectBillsUploadInputDto()
        {
        }
    }
}
