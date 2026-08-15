namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsUploadInputDto
    {
        public string Base64String { get; set; }
        public string CompressedFileExtension { get; set; }
        public string CompressedFileName { get; set; }
        public string CityCode { get; set; }
        public CollectBillsUploadInputDto(string base64String, string compressedFileExtension, string compressedFileName, string cityCode)
        {
            Base64String = base64String;
            CompressedFileExtension = compressedFileExtension;
            CompressedFileName = compressedFileName;
            CityCode = cityCode;
        }
        public CollectBillsUploadInputDto()
        {
        }
    }
}
