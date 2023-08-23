namespace Apps.ModernMT.Models.LanguageDetection.Requests;

public class DetectMultipleLanguagesRequest
{
    public IEnumerable<string> Texts { get; set; }
}