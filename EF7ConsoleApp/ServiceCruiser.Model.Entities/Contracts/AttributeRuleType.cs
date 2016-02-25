namespace ServiceCruiser.Model.Entities.Contracts
{
    public enum AttributeRuleType
    {
        ShowOnAllTrue = 0,
        ShowOnAllFalse = 1,
        ShowOnOneTrue = 2,
        ShowOnOneFalse = 3,
        ShowOnAllTrueWithResultCodes = 4,
        ShowOnAllFalseWithResultCodes = 5,
        ShowOnOneTrueWithResultCodes = 6,
        ShowOnOneFalseWithResultCodes = 7,
        ShowOnAllTrueWithoutResultCodes = 8,
        ShowOnAllFalseWithoutResultCodes = 9,
        ShowOnOneTrueWithoutResultCodes = 10,
        ShowOnOneFalseWithoutResultCodes = 11,
        Expression = 12
    }
}
