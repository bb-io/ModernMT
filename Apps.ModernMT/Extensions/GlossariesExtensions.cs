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
        // Note: this may not be a correct conversion for ModernMT language codes.
        var modernMtlanguageCodes = languages.Select(x => x.Split('-').First());
        result.Append("tuid,")
            .Append(string.Join(',', modernMtlanguageCodes))
            .Append(Environment.NewLine);

        var counter = 1;
        glossary.ConceptEntries.ToList().ForEach(entry =>
        {
            if (entry.LanguageSections.Count() <= 1)
                return;

            result.Append($"{counter++},");

            languages.ForEach(lang =>
            {
                var section = entry.LanguageSections.FirstOrDefault(x => x.LanguageCode == lang);
                var term = section?.Terms.First().Term.Replace(",", string.Empty);
                result.Append(term ?? string.Empty);

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