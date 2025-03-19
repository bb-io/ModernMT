using Apps.ModernMT.Actions;
using Apps.ModernMT.Models.LanguageDetection.Requests;
using Tests.ModernMT.Base;

namespace Tests.ModernMT;

[TestClass]
public class LanguageDetectionActionsTests : TestBase
{
    private const string ExampleText = "Hallo! Dit is een tekst in mijn eigen taal";

    [TestMethod]
    public void DetectLanguage_DutchText_ReturnsCorrectLanguageCode()
    {
        var actions = new LanguageDetectionActions(InvocationContext);

        var result = actions.DetectLanguage(new DetectLanguageRequest { Text = ExampleText });
        Assert.IsTrue(result.Language == "nl");
    }

    [TestMethod]
    public void DetectLanguage_EmptyText_ThrowsMisconfigurationException()
    {
        var actions = new LanguageDetectionActions(InvocationContext);
        Throws.MisconfigurationException(() => actions.DetectLanguage(new DetectLanguageRequest { Text = string.Empty }));
    }
}
