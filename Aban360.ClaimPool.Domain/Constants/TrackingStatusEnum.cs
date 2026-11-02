namespace Aban360.ClaimPool.Domain.Constants
{
    public enum TrackingStatusEnum : int
    {
        RequestRegistered = 0,
        SetAssessmentTime = 10,
        ExamineResultSet = 20,
        ReAssessment = 15,
        CalculationConfirmed = 60,
        ReCalculation = 65,
        AmountIsConfirmed = 75,
        Registered = 90,
        AmountConfirmed = 95,
        SetAssessmentResult = 110,
        SeenByAssessment = 150,
        ReferredToIndividual = 160,
        Deleted = 90000,
        Archived = 90003,
    }
}
