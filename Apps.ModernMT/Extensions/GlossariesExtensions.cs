using System.Text;
using Blackbird.Applications.Sdk.Glossaries.Utils.Models.Dtos;

namespace Apps.ModernMT.Extensions;

public static class GlossariesExtensions
{
    public static string ToModernMtCsv(this Glossary glossary)
    {
        var result = new StringBuilder();

        var languages = glossary.ConceptEntries
                .SelectMany(x => x.LanguageSections.Select(x => x.LanguageCode))
                .Distinct()
                .ToList();
        result.Append("tuid,")
            .Append(string.Join(',', languages))
            .Append(Environment.NewLine);

        var counter = 1;
        glossary.ConceptEntries.ToList().ForEach(entry =>
        {
            result.Append($"{counter++},");
            languages.ForEach(lang =>
            {
                var section = entry.LanguageSections.FirstOrDefault(x => x.LanguageCode == lang);
                result.Append(section?.Terms.First().Term ?? string.Empty);

                if (languages.IndexOf(lang) + 1 == languages.Count)
                {
                    result.Append(Environment.NewLine);
                    return;
                }

                result.Append(",");
            });
        });

        return result.ToString();
    }
}