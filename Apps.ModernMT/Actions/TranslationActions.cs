using Apps.ModernMT.Models.Translations.Requests;
using Apps.ModernMT.Models.Translations.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using ModernMT.Model;
using ModernMT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.ModernMT.Actions
{
    [ActionList]
    public class TranslationActions
    {
        [Action("Translate text", Description = "Translate into specified language")]
        public TranslationResponse TranslateIntoLanguage(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
            [ActionParameter] TranslationRequest input)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProvider);
            var translation = mmt.Translate(input.SourceLanguage, input.TargetLanguage, input.Text);
            return new TranslationResponse()
            {
                TranslatedText = translation.TranslationText
            };
        }

        [Action("Translate multiple texts", Description = "Translate multiple texts into specified language")]
        public MultipleTranslationResponse TranslateMultiple(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
            [ActionParameter] MultipleTranslationRequest input)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProvider);
            var translations = mmt.Translate(input.SourceLanguage, input.TargetLanguage, input.Texts);
            return new MultipleTranslationResponse()
            {
                TranslatedTexts = translations.Select(t => t.TranslationText).ToList()
            };
        }

        [Action("Translate text with hints", Description = "Translate text with hints")]
        public TranslationResponse TranslateWithHints(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
            [ActionParameter] TranslationWithHintsRequest input)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProvider);
            var translation = mmt.Translate(input.SourceLanguage, input.TargetLanguage, input.Text, input.Hints.ToArray());
            return new TranslationResponse()
            {
                TranslatedText = translation.TranslationText
            };
        }

        [Action("Translate text with context", Description = "Translate text with specified context")]
        public TranslationResponse TranslateWithContext(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
            [ActionParameter] TranslationWithContextRequest input)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProvider);
            var translation = mmt.Translate(input.SourceLanguage, input.TargetLanguage, input.Text, null, input.Context);
            return new TranslationResponse()
            {
                TranslatedText = translation.TranslationText
            };
        }

        [Action("Get translation options", Description = "Get translation options")]
        public TranslationOptionsResponse GetTranslationOptions(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
            [ActionParameter] TranslationOptionsRequest input)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProvider);
            var translation = mmt.Translate(input.SourceLanguage, input.TargetLanguage, input.Text, null, null, new TranslateOptions()
            {
                AltTranslations = input.NumberOfOptions
            });
            return new TranslationOptionsResponse()
            {
                TranslatedText = translation.TranslationText,
                AlternativeOptions = translation.AltTranslations.ToList()
            };
        }
    }
}
