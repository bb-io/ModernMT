using Apps.ModernMT.Actions;
using Apps.ModernMT.Models.LanguageDetection.Requests;
using Apps.ModernMT.Models.Translations.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.ModernMT.Base;

namespace Tests.ModernMT;

[TestClass]
public class TranslateTests : TestBase
{
    private const string ExampleText = "Hallo! Dit is een tekst in mijn eigen taal";

    [TestMethod]
    public void Standard_translate_works_correctly()
    {
        var actions = new TranslationActions(InvocationContext, FileManager);

        var result = actions.TranslateIntoLanguage(new TranslationRequest { Text = ExampleText, SourceLanguage = "nl", TargetLanguage = "en" });
        Assert.IsTrue(result.TranslatedText != null);
    }

    [TestMethod]
    public void Same_language_throws_misconfiguration()
    {
        var actions = new TranslationActions(InvocationContext, FileManager);
        Throws.MisconfigurationException(() => actions.TranslateIntoLanguage(new TranslationRequest { Text = ExampleText, SourceLanguage = "nl", TargetLanguage = "nl" }));
    }

    [TestMethod]
    public void Empty_text_throws_misconfiguration()
    {
        var actions = new TranslationActions(InvocationContext, FileManager);
        Throws.MisconfigurationException(() => actions.TranslateIntoLanguage(new TranslationRequest { Text = string.Empty, SourceLanguage = "nl", TargetLanguage = "en" }));
    }
}
