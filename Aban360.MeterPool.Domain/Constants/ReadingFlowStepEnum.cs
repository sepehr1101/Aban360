namespace Aban360.MeterPool.Domain.Constants
{
    public enum ReadingFlowStepEnum
    {
        CreatePath = 1,
        DownloadByReader = 2,
        StartReading = 3,
        Upload = 4,
        DownloadFile = 5,
        ReturnToCreatePath = 6,
        ReturnToReading = 7,
        ReturnToUpload = 8,
        EditCreatedPath = 9,
        RemoveFile = 10
    }
}
