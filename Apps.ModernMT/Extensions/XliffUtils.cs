using Blackbird.Xliff.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Apps.ModernMT.Extensions
{
    public class XliffUtils
    {
        public static ParsedXliff ParseXLIFF(Stream file)
        {
            var xliffDocument = XDocument.Load(file);
            XNamespace defaultNs = xliffDocument.Root.GetDefaultNamespace();
            var tus = new List<TranslationUnit>();

            foreach (var tu in (from tu in xliffDocument.Descendants(defaultNs + "trans-unit") select tu).ToList())
            {
                string src = "";
                if (tu.Elements().Any(x => x.Name == defaultNs + "seg-source")) 
                { src = RemoveExtraNewLines(Regex.Replace(tu.Element(defaultNs + "seg-source").ToString(), @"</?seg-source(.*?)>", @"")); }
                    
                tus.Add(new TranslationUnit
                {

                    Source = src == ""? RemoveExtraNewLines(Regex.Replace(tu.Element(defaultNs + "source").ToString(), @"</?source(.*?)>", @"")) : src,
                    Target = RemoveExtraNewLines(Regex.Replace(tu.Element(defaultNs + "target").ToString(), @"</?target(.*?)>", @"")),
                    Id = tu.Attribute("id").Value
                });
            }

            return new ParsedXliff
            {
                SourceLanguage = xliffDocument.Root?.Element(defaultNs + "file")?.Attribute("source-language")?.Value,
                TargetLanguage = xliffDocument.Root?.Element(defaultNs + "file")?.Attribute("target-language")?.Value,
                TranslationUnits = tus
            };
        }
        public static string RemoveExtraNewLines(string originalString)
        {
            if (!string.IsNullOrWhiteSpace(originalString))
            {
                var to_modify = originalString;
                to_modify = Regex.Replace(to_modify, @"\r\n(\s+)?", "", RegexOptions.Multiline);
                return to_modify;
            }
            else
            {
                return string.Empty;
            }
        }

        public static Stream UpdateOriginalFile(Stream fileStream, List<string> results)
        {
            string fileContent;
            Encoding encoding;

            using (StreamReader inFileStream = new StreamReader(fileStream))
            {
                encoding = inFileStream.CurrentEncoding;
                fileContent = inFileStream.ReadToEnd();
            }

            var tus = Regex.Matches(fileContent, @"<trans-unit [\s\S]+?</trans-unit>").Select(x => x.Value);
            if (tus.Count() != results.Count()) throw new Exception("Translated texts is different from number of segments in file.");
            foreach (var tu in tus.Zip(results))
            {
                var newtu = Regex.Replace(tu.First, "(<target(.*?)>)([\\s\\S]+?)(</target>)", "${1}" + tu.Second + "${4}");
                fileContent = Regex.Replace(fileContent, Regex.Escape(tu.First), newtu);
    
            }
            return new MemoryStream(encoding.GetBytes(fileContent));
        }

    }

    public class ParsedXliff
    {
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }

        public List<TranslationUnit> TranslationUnits { get; set; }

    }
}

