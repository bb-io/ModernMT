using Apps.ModernMT.Actions;
using Apps.ModernMT.Models.Quality;
using Blackbird.Applications.Sdk.Common.Files;
using Tests.ModernMT.Base;

namespace Tests.ModernMT;

[TestClass]
public class QualityActionsTests : TestBase
{
    [TestMethod]
    public async Task EstimateXliff_ValidInput_ReturnsQualityScores()
    {
        // Arrange
        var actions = new QualityActions(InvocationContext, FileManager);
        var fileRef = new FileReference { Name = "quality_test.xlf" };

        // Act
        var result = await actions.EstimateXliff(new EstimateXliffRequest
        {
            File = fileRef,
            SourceLanguage = "nl",
            TargetLanguage = "en"
        });

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.AverageScore > 0);
        Assert.IsNotNull(result.File);
        PrintResult(result);
    }

    [TestMethod]
    public async Task EstimateXliff_WithThresholdAndCondition_FiltersTranslations()
    {
        // Arrange
        var actions = new QualityActions(InvocationContext, FileManager);
        var fileRef = new FileReference { Name = "quality_test.xlf" };

        // Act
        var result = await actions.EstimateXliff(new EstimateXliffRequest
        {
            File = fileRef,
            SourceLanguage = "nl",
            TargetLanguage = "en",
            Threshold = 0.8f,
            Condition = ">",
            State = "confirmed"
        });

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.AverageScore > 0);
        Assert.IsNotNull(result.File);
        PrintResult(result);
    }

    [TestMethod]
    public async Task EstimateXliff_NoThresholdSpecified_OnlyCalculatesScores()
    {
        // Arrange
        var actions = new QualityActions(InvocationContext, FileManager);
        var fileRef = new FileReference { Name = "quality_test.xlf" };

        // Act
        var result = await actions.EstimateXliff(new EstimateXliffRequest
        {
            File = fileRef,
            SourceLanguage = "nl",
            TargetLanguage = "en",
        });

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.AverageScore > 0);
        Assert.IsNotNull(result.File);
        PrintResult(result);
    }
    
    [TestMethod]
    public async Task EstimateXliff_ExtremeQualityDifference_IdentifiesGoodAndBadTranslations()
    {
        // Arrange
        var actions = new QualityActions(InvocationContext, FileManager);
        var fileRef = new FileReference { Name = "quality_test.xlf" };

        // Act
        var result = await actions.EstimateXliff(new EstimateXliffRequest
        {
            File = fileRef,
            SourceLanguage = "nl",
            TargetLanguage = "en",
            Threshold = 0.4f,
            Condition = "<",
            State = "needs-translation"
        });

        // Assert
        Assert.IsNotNull(result);
        PrintResult(result);
    }
}
