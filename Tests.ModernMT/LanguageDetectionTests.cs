using Apps.ModernMT.Actions;
using Apps.ModernMT.Connections;
using Apps.ModernMT.Models.LanguageDetection.Requests;
using Blackbird.Applications.Sdk.Common.Authentication;
using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.ModernMT.Base;

namespace Tests.ModernMT;

[TestClass]
public class LanguageDetectionTests : TestBase
{
    private const string ExampleText = "Hallo! Dit is een tekst in mijn eigen taal";

    [TestMethod]
    public void Dutch_language_correctly_detected()
    {
        var actions = new LanguageDetectionActions(InvocationContext);

        var result = actions.DetectLanguage(new DetectLanguageRequest { Text = ExampleText });
        Assert.IsTrue(result.Language == "nl");
    }

    [TestMethod]
    public void Empty_text_throws_misconfiguration()
    {
        var actions = new LanguageDetectionActions(InvocationContext);
        Throws.MisconfigurationException(() => actions.DetectLanguage(new DetectLanguageRequest { Text = string.Empty }));
    }
}
