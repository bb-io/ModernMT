using Apps.ModernMT.Actions;
using Apps.ModernMT.Models.Translations.Requests;
using Blackbird.Applications.Sdk.Common.Files;
using Tests.ModernMT.Base;

namespace Tests.ModernMT;

[TestClass]
public class TranslationActionsTests : TestBase
{
    private const string ExampleText = "Hallo! Dit is een tekst in mijn eigen taal";

    [TestMethod]
    public void TranslateIntoLanguage_ValidInput_ReturnsTranslation()
    {
        var actions = new TranslationActions(InvocationContext, FileManager);

        var result = actions.TranslateIntoLanguage(new TranslationRequest { Text = ExampleText, SourceLanguage = "nl", TargetLanguage = "en" });
        Assert.IsTrue(result.TranslatedText != null);
        Console.WriteLine(result.TranslatedText);
    }

    [TestMethod]
    public void TranslateIntoLanguage_NoSource_ReturnsTranslation()
    {
        var actions = new TranslationActions(InvocationContext, FileManager);

        var result = actions.TranslateIntoLanguage(new TranslationRequest { Text = ExampleText, TargetLanguage = "en" });
        Assert.IsTrue(result.TranslatedText != null);
        Console.WriteLine(result.TranslatedText);
    }

    [TestMethod]
    public void TranslateIntoLanguage_SameLanguage_ThrowsMisconfigurationException()
    {
        var actions = new TranslationActions(InvocationContext, FileManager);
        Throws.MisconfigurationException(() => actions.TranslateIntoLanguage(new TranslationRequest { Text = ExampleText, SourceLanguage = "nl", TargetLanguage = "nl" }));
    }

    [TestMethod]
    public void TranslateIntoLanguage_EmptyText_ThrowsMisconfigurationException()
    {
        var actions = new TranslationActions(InvocationContext, FileManager);
        Throws.MisconfigurationException(() => actions.TranslateIntoLanguage(new TranslationRequest { Text = string.Empty, SourceLanguage = "nl", TargetLanguage = "en" }));
    }

    [TestMethod]
    public async Task TranslateXliff_ValidInput_ReturnsTranslatedFile()
    {
        var actions = new TranslationActions(InvocationContext, FileManager);

        // Set up file reference
        var fileRef = new FileReference { Name = "sample.xlf" };

        var result = await actions.TranslateXliff(
            new TranslateXliffRequest
            {
                File = fileRef,
                SourceLanguage = "nl",
                TargetLanguage = "en"
            },
            5);

        Assert.IsNotNull(result.File);
        Assert.IsTrue(result.BilledCharacters > 0);
    }

    [TestMethod]
    public async Task TranslateXliff_SameLanguage_ThrowsMisconfigurationException()
    {
        var actions = new TranslationActions(InvocationContext, FileManager);
        var fileRef = new FileReference { Name = "sample.xlf" };

        await Throws.MisconfigurationExceptionAsync(async () =>
            await actions.TranslateXliff(
                new TranslateXliffRequest
                {
                    File = fileRef,
                    SourceLanguage = "nl",
                    TargetLanguage = "nl"
                },
                5)
        );
    }

    [TestMethod]
    public async Task Translate_file_xliff()
    {
        var actions = new TranslationActions(InvocationContext, FileManager);

        // Set up file reference
        var fileRef = new FileReference { Name = "contentful.html" };

        var result = await actions.TranslateFile(
            new TranslateFileRequest
            {
                File = fileRef,
                TargetLanguage = "nl"
            });

        Assert.IsNotNull(result.File);
        Assert.IsTrue(result.BilledCharacters > 0);
    }

    [TestMethod]
    public async Task Translate_file_html()
    {
        var actions = new TranslationActions(InvocationContext, FileManager);

        // Set up file reference
        var fileRef = new FileReference { Name = "contentful.html" };

        var result = await actions.TranslateFile(
            new TranslateFileRequest
            {
                File = fileRef,
                TargetLanguage = "nl",
                OutputFileHandling = "original",
            });

        Assert.IsNotNull(result.File);
        Assert.IsTrue(result.BilledCharacters > 0);
    }
}
