namespace Aban360.ClaimPool.Domain.Constants
{
    public enum RequestStatusEnum : int
    {
        RequestIsRegisterd = 0,
        ExamineTimeSet = 10,
        ReExamineRequired = 15,
        CalculationConfirmd = 60,
        ReCalculateRequired = 65,
        RadifSpecified = 70,
        AmountIsConfirmed = 75,
        RegisterationDetermined = 90,
        SetExaminationResult = 110,
        SeenByExaminer = 150,
        SoftDeleted = 90_000,
        FullDeleted = 90_001,
        SkipSpecifyRadif = 90_002,
        Archived = 90_003
    }
}